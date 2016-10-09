using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIKBCI.Common
{
    public class NBDataReader
    {
        public int Channels { get;set; }
        public int SampleRate { get; set; }
        public int Resolution { get; set; }
        public string Notch { get; set; }

        public List<NBDataItem> GetDataItems()
        {
            System.Globalization.NumberFormatInfo NF = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
            StreamReader SR = new StreamReader(@"c:\Users\anon\Desktop\teszt_212748_raw.txt");
            List<NBDataItem> output = new List<NBDataItem>();
            string line1 = SR.ReadLine().Replace(" ", "");
            string[] fields = line1.Split('\t');
            Channels = int.Parse(fields[1]);
            SampleRate = int.Parse(fields[3]);
            Resolution = int.Parse(fields[5]);
            Notch = fields[7];
            string line2 = SR.ReadLine().Replace(" ", "");
            int seq = 0;
            while (!SR.EndOfStream)
            {
                string line = SR.ReadLine().Replace(" ", "").Replace(",", ".");
                fields = line.Split('\t');
                double timestamp = int.Parse(fields[0].Substring(0, 2)) * 3600 +
                    int.Parse(fields[0].Substring(3, 2)) * 60 +
                    int.Parse(fields[0].Substring(6, 2)) +
                    double.Parse(fields[0].Substring(9, 4)) / 10000;

                NBDataItem item = new NBDataItem()
                {
                     Seq=seq,
                     TimeStr=fields[0], 
                     TimeStamp=timestamp,
                     EventNo=int.Parse(fields[Channels+1]),
                     Channels=new double[Channels]
                };
                for (int i = 0; i < Channels; i++)
                {
                    item.Channels[i] = double.Parse(fields[i + 1], NF);
                }
                item.OneChannel = item.Channels[0];
                output.Add(item);
                seq++;
            }
            return output;
        }
    }
}
