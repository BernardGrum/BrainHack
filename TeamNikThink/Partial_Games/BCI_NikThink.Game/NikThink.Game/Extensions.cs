using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NikThink.Game
{
    public static class Extensions
    {
        public  static object GetCopy(this object input)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, input);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }
    }
}
