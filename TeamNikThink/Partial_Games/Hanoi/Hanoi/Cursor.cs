using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanoi
{
    class Cursor
    {
        int index;
        ShroomObject selectedObj;
        ShroomObject currentObj;

        public Cursor()
        {
            index = 0;
            selectedObj = null;
            currentObj = null;
        }

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        public ShroomObject SelectedObj
        {
            get
            {
                return selectedObj;
            }

            set
            {
                selectedObj = value;
            }
        }

        public ShroomObject CurrentObj
        {
            get
            {
                return currentObj;
            }

            set
            {
                currentObj = value;
            }
        }
    }
}
