using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NikThink.Game
{
    class LabyrinthVisualHost : VisualHost
    {
        public  int TileCount { get; private set; }
        ImageBrush player;
        ImageBrush goal;
            
        int tileSize { get { return (int)this.Height / TileCount; } }

       
        int marginSize;

        public LabyrinthVisualHost()
        {
            TileCount = int.Parse(ConfigurationManager.AppSettings["tileCount"]);
            marginSize = int.Parse(ConfigurationManager.AppSettings["marginSize"]);
            player = new ImageBrush(new BitmapImage(new Uri(@"..\..\img\mouse.jpg", UriKind.Relative)));
            goal = new ImageBrush(new BitmapImage(new Uri(@"..\..\img\cheese.png", UriKind.Relative)));
        }
        
        public override void Draw(object drawable)
        {
            if (drawable is GameDrawable)
            {
                var visual = new DrawingVisual();
                children.Clear();
                children.Add(visual);

                using (var dc = visual.RenderOpen())
                {
                    this.DrawMap(dc, (drawable as GameDrawable).Map);
                    this.DrawPlayer(dc, (drawable as GameDrawable).Player);
                }
                
                return;
            }
        }

        private void DrawPlayer(DrawingContext dc, Player pl)
        {
            
            dc.DrawRectangle(player, new Pen(Brushes.Red, 0), new Rect(pl.PosX * tileSize + marginSize, pl.PosY * tileSize + marginSize, tileSize - 2* marginSize, tileSize - 2 * marginSize));
        }

        private void DrawMap(DrawingContext dc, Map map)
        {
            
            foreach (MapPiece mp in map.MapPieces)
            {
                double x = (mp.PosX) * tileSize - marginSize;
                double y = (mp.PosY) * tileSize - marginSize;
                double tm2 = tileSize + 2 * marginSize;
                double m2 = 2 * marginSize;
                if (mp.UpperBound)
                    dc.DrawRectangle(Brushes.Black, new Pen(Brushes.Red, 0),
                        new Rect(x, y, tm2, m2));

                if (mp.LowerBound)
                    dc.DrawRectangle(Brushes.Black, new Pen(Brushes.Red, 0),
                        new Rect(x, y + tileSize, tm2, m2));

                if (mp.LeftBound)
                    dc.DrawRectangle(Brushes.Black, new Pen(Brushes.Red, 0),
                        new Rect(x, y, m2, tm2));

                if (mp.RightBound)
                    dc.DrawRectangle(Brushes.Black, new Pen(Brushes.Red, 0),
                        new Rect(x + tileSize, y, m2, tm2));

                if (mp.Sequence!=null)
                {var ft = new FormattedText(mp.Sequence.ToString(), CultureInfo.GetCultureInfo("en-us"),
                                              FlowDirection.LeftToRight,
                                              new Typeface(new FontFamily("Arial").ToString()),
                                              12, Brushes.Black);
                    dc.DrawText(ft, new Point(x +  m2, y+ m2));
                }

                dc.DrawRectangle(goal, new Pen(Brushes.Red, 0), new Rect((TileCount-1)  * tileSize + marginSize, (TileCount - 1) * tileSize + marginSize, tileSize - 2 * marginSize, tileSize - 2 * marginSize));
            }
        }
    } }