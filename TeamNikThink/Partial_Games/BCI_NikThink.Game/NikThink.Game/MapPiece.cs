using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikThink.Game
{
    [Serializable]
    public class MapPiece : GameItem
    {
        static Random R = new Random();

        public MapPiece(int X, int Y) : base(X,Y)
        {
            //int p2 = 20;
            //int p = 10;
            //UpperBound = R.Next(0, p2)  <  p;
            //LowerBound = R.Next(0, p2)  <  p;
            //LeftBound =  R.Next(0, p2)  <  p;
            //RightBound = R.Next(0, p2)  <  p;
        }
        public string Sequence;
        public bool UpperBound { get; set; }
        public bool LowerBound { get; set; }
        public bool LeftBound { get; set; }
        public bool RightBound { get; set; }
    }

  
}
