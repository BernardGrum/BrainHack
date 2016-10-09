using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGeneration;

namespace NikThink.Game
{
    public enum Direction { Up, Down, Left, Right }

    public class Map
    {
        static Random R = new Random();

        public MapPiece[,] MapPieces;

        public Map(int Size)
        {
            MapPieces = this.GenerateMap(Size);
        }

        public MapPiece[,] GenerateMap(int Size)
        {
            var map = new MapPiece[Size, Size];
            var maze = new Maze(Size, Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    map[i, j] = new MapPiece(i, j);

                    if (maze[i, j].HasFlag(CellState.Top))
                        map[i, j].UpperBound = true;

                    if (maze[i, j].HasFlag(CellState.Bottom))
                        map[i, j].LowerBound = true;

                    if (maze[i, j].HasFlag(CellState.Left))
                        map[i, j].LeftBound = true;

                    if (maze[i, j].HasFlag(CellState.Right))
                        map[i, j].RightBound = true;
                }
            }
            return map;
        }
    }
}
