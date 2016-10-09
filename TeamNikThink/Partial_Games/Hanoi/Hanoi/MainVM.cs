using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Hanoi
{
    class MainVM : INotifyPropertyChanged
    {
        static MainVM entity;

        public static MainVM Get()
        {
            if (entity == null) entity = new MainVM();
            return entity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void onPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private ObservableCollection<ShroomObject> towerLeft;

        public ObservableCollection<ShroomObject> TowerLeft
        {
            get { return towerLeft; }
            set { towerLeft = value; onPropertyChanged("towerLeft"); }
        }

        private ObservableCollection<ShroomObject> towerRight;

        public ObservableCollection<ShroomObject> TowerRight
        {
            get { return towerRight; }
            set { towerRight = value; onPropertyChanged("towerRight"); }
        }

        private ObservableCollection<ShroomObject> towerCenter;

        public ObservableCollection<ShroomObject> TowerCenter
        {
            get { return towerCenter; }
            set { towerCenter = value; onPropertyChanged("towerCenter"); }
        }

        private List<ObservableCollection<ShroomObject>> towers;

        public List<ObservableCollection<ShroomObject>> Towers
        {
            get { return towers; }
            set { towers = value; onPropertyChanged("towerCenter"); }
        }

        public MainVM()
        {
            towers = new List<ObservableCollection<ShroomObject>>();
            
            towerLeft = new ObservableCollection<ShroomObject>();
            towerRight = new ObservableCollection<ShroomObject>();
            towerCenter = new ObservableCollection<ShroomObject>();

            towers.Add(towerLeft);
            towers.Add(towerCenter);
            towers.Add(towerRight);
        }
    }
}
