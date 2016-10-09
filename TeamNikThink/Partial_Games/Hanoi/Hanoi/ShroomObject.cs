using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hanoi
{
    class ShroomObject : Image
    {
        bool isSelected;
        Border objectBorder;
        int column;
        int row;

        public ShroomObject()
        {
            isSelected = false;
            ObjectBorder = new Border();
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
            }
        }

        public Border ObjectBorder
        {
            get
            {
                return objectBorder;
            }

            set
            {
                objectBorder = value;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
            }
        }

        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
        }
    }
}
