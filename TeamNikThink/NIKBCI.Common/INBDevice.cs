using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIKBCI.Common
{
    /// <summary>
    /// The main interface - this is what WPF apps should use!
    /// </summary>
    public interface INBDevice
    {
        /// <summary>
        /// This event will be fired if an action arrives from the device
        /// </summary>
        event EventHandler<NBResult> ActionArrived;

        /// <summary>
        /// Use this method to ask the device to calibrate for an action
        /// Returns "NULL" on error!
        /// </summary>
        /// <param name="action">The action to calibrate for</param>
        /// <returns>Calibration output feedback, or NULL in case of calibration error</returns>
        Task<string> TrainActionAsync(NBAction action);

        /// <summary>
        /// Must call this method to initialize
        /// </summary>
        void StartEverything();

        /// <summary>
        /// Must call this method to UNinitialize
        /// </summary>
        void StopEverything();

        /// <summary>
        /// Save the trained action-associations to a file
        /// </summary>
        /// <param name="filename">The file name</param>
        void SaveTrainedData(string filename);

        /// <summary>
        /// Load the trained action-associations from a file
        /// </summary>
        /// <param name="filename">The file name</param>
        void LoadTrainedData(string filename);
    }
}
