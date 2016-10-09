using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIKBCI.Common
{
    /// <summary>
    /// The result of an interaction from the device
    /// </summary>
    public class NBResult
    {
        /// <summary>
        /// The action associated with the interaction
        /// </summary>
        public NBAction Action { get; set; }

        /// <summary>
        /// Textual description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Guess what
        /// </summary>
        /// <returns>No idea</returns>
        public override string ToString()
        {
            return String.Format("ACTION {0}, DESC: {1}", Action, Description);
        }
    }
}
