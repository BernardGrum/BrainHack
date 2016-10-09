using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIKBCI.Common
{
    public class NBDataItem
    {
        public string TimeStr { get; set; }
        public double TimeStamp { get; set; }
        public int Seq { get; set; }
        public double[] Channels { get; set; }
        public double OneChannel;
        public int EventNo { get; set; }
    }
}
