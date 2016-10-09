using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NikThink.Game
{
    abstract public class VisualHost : FrameworkElement
    {
        protected override int VisualChildrenCount
        {
            get { return children.Count; }
        }

        protected VisualCollection children;

        protected VisualHost()
        {
            children = new VisualCollection(this);
        }

        public abstract void Draw(object drawable);

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            return children[index];
        }
     
    }

}
