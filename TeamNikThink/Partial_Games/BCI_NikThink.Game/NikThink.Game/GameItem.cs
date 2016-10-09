using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikThink.Game
{
    [Serializable]
    public abstract class GameItem
    {
        public GameItem(int X, int Y)
        {
            PosX = X;
            PosY = Y;
        }
        public double PosX { get; set; }
        public double PosY { get; set; }
    }
}
