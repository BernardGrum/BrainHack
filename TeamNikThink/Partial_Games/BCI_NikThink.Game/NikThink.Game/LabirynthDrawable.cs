using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikThink.Game
{
    class GameDrawable
    {
        public Map Map { get; set; }
        public Player Player { get; set; }

        public GameDrawable(Map M, Player P)
        {
            Map = M;
            Player = P;
        }
    }
}
