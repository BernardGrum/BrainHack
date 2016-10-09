using NIKBCI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIKBCI.Keyboard
{
    /// <summary>
    /// Class that demonstrates the usage of INBDevice
    /// </summary>
    public class NBKeyboard : INBDevice
    {
        [DllImport("user32.dll")] 
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        Dictionary<Keys, NBAction> actions;
        public event EventHandler<NBResult> ActionArrived;

        Keys trainKey = Keys.Zoom;
        
        public async Task<string> TrainActionAsync(NBAction action)
        {
            Keys key = await AsyncWaitForKey();
            if (actions.ContainsKey(key))
            {
                actions[key] = action;
            }
            else
            {
                actions.Add(key, action);
            }
            return action + " [bound to] " + key;
        }

        public async Task<Keys> AsyncWaitForKey()
        {
            trainKey = Keys.Zoom;
            while (trainKey == Keys.Zoom)
            {
                await Task.Delay(50);
            }
            return trainKey;
        }

        public void StartEverything()
        {
             actions = new Dictionary<Keys, NBAction>();
             KeyboardHandler.Init();
             KeyboardHandler.KeyDown += keybHandler_KeyDown;
        }

        void keybHandler_KeyDown(object sender, Keys e)
        {
            trainKey = e;
            if (ActionArrived != null)
            {
                NBResult result = new NBResult();
                result.Description = "Key: " + e;
                result.Action=actions.ContainsKey(e) ? actions[e] : NBAction.Unknown;
                ActionArrived(this, result);
            }
        }

        public void StopEverything()
        {
            KeyboardHandler.Uninit();
            KeyboardHandler.KeyDown -= keybHandler_KeyDown;
        }


        public void SaveTrainedData(string filename)
        {
            // NOT IMPLEMENTED
        }

        public void LoadTrainedData(string filename)
        {
            actions.Add(Keys.W, NBAction.Up);
            actions.Add(Keys.S, NBAction.Down);
            actions.Add(Keys.A, NBAction.Left);
            actions.Add(Keys.D, NBAction.Right);
            actions.Add(Keys.P, NBAction.Fire);
        }
    }
}
