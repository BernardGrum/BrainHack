using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace NikThink.Game
{
    class GameManager
    {
        Map map;
        VisualHost vh;
        Player pl;

        public GameManager(LabyrinthVisualHost vh)
        {
            this.vh = vh;
            map = new Map(vh.TileCount);
            pl = new Player(0,0);
            vh.Draw(new GameDrawable(map, pl)); 
        }

        public void Move(Key k)
        {
            pl.Move(k, map);
            vh.Draw(new GameDrawable(map, pl));
        }
    }
}
