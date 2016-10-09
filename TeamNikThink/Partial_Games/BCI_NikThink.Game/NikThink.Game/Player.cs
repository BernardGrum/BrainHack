using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NikThink.Game
{
    public class Player: GameItem
    {
        public Player(int X, int Y) : base(X, Y)
        {

        }
        public void Move(Key k, Map m)
        {
            switch (k)
            {
                case Key.Up:
                    if (this.PosY>0 
                     && !m.MapPieces[(int)this.PosX, (int)this.PosY-1].LowerBound
                     && !m.MapPieces[(int)this.PosX, (int)this.PosY  ].UpperBound)
                    {
                        this.PosY--;
                    }
                    break;
                case Key.Down:
                    if (this.PosY < m.MapPieces.GetLength(0) -1 
                     && !m.MapPieces[(int)this.PosX, (int)this.PosY + 1].UpperBound
                     && !m.MapPieces[(int)this.PosX, (int)this.PosY].LowerBound)
                    {
                        this.PosY++;
                    }
                    break;
                case Key.Left:
                    if (this.PosX > 0
                     && !m.MapPieces[(int)this.PosX-1, (int)this.PosY].RightBound
                     && !m.MapPieces[(int)this.PosX, (int)this.PosY].LeftBound)
                    {
                        this.PosX--;
                    }
                   
                    break;
                case Key.Right:
                    if (this.PosX < m.MapPieces.GetLength(1) -1 
                     && !m.MapPieces[(int)this.PosX + 1, (int)this.PosY].LeftBound
                     && !m.MapPieces[(int)this.PosX, (int)this.PosY].RightBound)
                    {
                        this.PosX++;
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }
}
