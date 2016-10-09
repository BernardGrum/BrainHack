using NIKBCI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NIKBCI.WpfTester
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        ObservableCollection<NBDataItem> chartPointCollection;

        public ObservableCollection<NBDataItem> ChartPointCollection
        {
            get { return chartPointCollection; }
            set { chartPointCollection = value; OnPropertyChanged(); }
        }


    }
}
