using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hanoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVM vm;
        Cursor cursor;

        int[] towerTops = new int[3];

        public MainWindow()
        {
            InitializeComponent();
            vm = MainVM.Get();
            cursor = new Cursor();
            Initialize_GameField();
            towerTops[0] = 0;
            towerTops[1] = 3;
            towerTops[2] = 3;
            ChangeSelection(0, 0);
            this.KeyDown += new KeyEventHandler(Window_KeyDown);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left: MoveLeft();
                    break;
                case Key.Right: MoveRight();
                    break;
                case Key.Enter:
                    if (cursor.CurrentObj == null)
                    { SelectObject(); }
                    else
                    { InsertObject(); }
                    break;
            }
        }

        private void MoveLeft()
        {
            if (cursor.SelectedObj.Column == 0)
            {
                return;
            }
            switch (cursor.SelectedObj.Column - 1)
            {
                case 0:
                    ChangeSelection(cursor.SelectedObj.Column - 1, towerTops[0]);
                    break;
                case 1:
                    ChangeSelection(cursor.SelectedObj.Column - 1, towerTops[1]);
                    break;
            }
        }

        private void MoveRight()
        {
            if (cursor.SelectedObj.Column == 2)
            {
                return;
            }
            switch (cursor.SelectedObj.Column + 1)
            {
                case 1:
                    ChangeSelection(cursor.SelectedObj.Column + 1, towerTops[1]);
                    break;
                case 2:
                    ChangeSelection(cursor.SelectedObj.Column + 1, towerTops[2]);
                    break;
            }
        }

        private void SelectObject()
        {
            if (cursor.SelectedObj.Visibility == Visibility.Visible)
            {
                cursor.CurrentObj = cursor.SelectedObj;
                cursor.CurrentObj.OpacityMask = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            }
        }

        private void InsertObject()
        {
            if(vm.Towers[cursor.SelectedObj.Column][cursor.CurrentObj.Row].Visibility == Visibility.Hidden)
            {
                if (towerTops[cursor.SelectedObj.Column] >= cursor.CurrentObj.Row)
                {
                    vm.Towers[cursor.SelectedObj.Column][cursor.CurrentObj.Row].Visibility = Visibility.Visible;
                    cursor.CurrentObj.Visibility = Visibility.Hidden;
                    cursor.CurrentObj.OpacityMask = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    cursor.CurrentObj = null;

                    ReIndexTop();
                }
            }
            else if(cursor.SelectedObj == cursor.CurrentObj)
            {
                cursor.CurrentObj.OpacityMask = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                cursor.CurrentObj = null;
            }
        }

        private void ReIndexTop()
        {
            for (int i = 0; i < 3; i++)
            {
                towerTops[i] = 3;
            }
            for (int i = 0; i < vm.Towers.Count; i++)
            {
                for (int j = 0; j < vm.Towers[i].Count; j++)
                {
                    if(vm.Towers[i][j].Visibility == Visibility.Visible)
                    {
                        towerTops[i] = j;
                        j = vm.Towers.Count;
                    }
                }
            }
        }

        private void ChangeSelection(int column, int row)
        {
            if(cursor.SelectedObj != null)
            {
                cursor.SelectedObj.ObjectBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            vm.Towers[column][row].ObjectBorder.BorderBrush = new SolidColorBrush(Colors.White);
            cursor.SelectedObj = vm.Towers[column][row];

        }

        private void Initialize_GameField()
        {

            for (int i = 0; i < 3; i++)
            {
                ShroomObject obj = new ShroomObject();
                obj.Width = 90;
                obj.Height = 60;
                obj.Visibility = Visibility.Hidden;
                obj.Stretch = Stretch.Fill;
                obj.Source = new BitmapImage(new Uri("/Resources/shroom_yellow.jpg", UriKind.Relative));
                obj.ObjectBorder.BorderThickness = new Thickness(3);
                obj.ObjectBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                obj.ObjectBorder.Width = obj.Width;
                obj.ObjectBorder.Height = obj.Height;
                obj.ObjectBorder.Visibility = Visibility.Visible;

                grid_gamefield.Children.Add(obj);
                obj.SetValue(Grid.ColumnProperty, i);
                obj.SetValue(Grid.RowProperty, 0);

                grid_gamefield.Children.Add(obj.ObjectBorder);
                obj.ObjectBorder.SetValue(Grid.ColumnProperty, i);
                obj.ObjectBorder.SetValue(Grid.RowProperty, 0);

                obj.Row = 0;

                switch (i)
                {
                    case 0: vm.TowerLeft.Add(obj);
                        obj.Visibility = Visibility.Visible;
                        obj.Column = 0;
                        break;
                    case 1: vm.TowerCenter.Add(obj);
                        obj.Column = 1;
                        break;
                    case 2: vm.TowerRight.Add(obj);
                        obj.Column = 2;
                        break;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                ShroomObject obj = new ShroomObject();
                obj.Width = 120;
                obj.Height = 60;
                obj.Visibility = Visibility.Hidden;
                obj.Stretch = Stretch.Fill;
                obj.Source = new BitmapImage(new Uri("Resources/shroom_blue.jpg", UriKind.Relative));
                obj.ObjectBorder.BorderThickness = new Thickness(3);
                obj.ObjectBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                obj.ObjectBorder.Width = obj.Width;
                obj.ObjectBorder.Height = obj.Height;
                obj.ObjectBorder.Visibility = Visibility.Visible;

                grid_gamefield.Children.Add(obj);
                obj.SetValue(Grid.ColumnProperty, i);
                obj.SetValue(Grid.RowProperty, 1);

                grid_gamefield.Children.Add(obj.ObjectBorder);
                obj.ObjectBorder.SetValue(Grid.ColumnProperty, i);
                obj.ObjectBorder.SetValue(Grid.RowProperty, 1);

                obj.Row = 1;

                switch (i)
                {
                    case 0:
                        vm.TowerLeft.Add(obj);
                        obj.Visibility = Visibility.Visible;
                        obj.Column = 0;
                        break;
                    case 1:
                        vm.TowerCenter.Add(obj);
                        obj.Column = 1;
                        break;
                    case 2:
                        vm.TowerRight.Add(obj);
                        obj.Column = 2;
                        break;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                ShroomObject obj = new ShroomObject();
                obj.Width = 150;
                obj.Height = 60;
                obj.Visibility = Visibility.Hidden;
                obj.Stretch = Stretch.Fill;
                obj.Source = new BitmapImage(new Uri("Resources/shroom_purple.jpg", UriKind.Relative));
                obj.ObjectBorder.BorderThickness = new Thickness(3);
                obj.ObjectBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                obj.ObjectBorder.Width = obj.Width;
                obj.ObjectBorder.Height = obj.Height;
                obj.ObjectBorder.Visibility = Visibility.Visible;

                grid_gamefield.Children.Add(obj);
                obj.SetValue(Grid.ColumnProperty, i);
                obj.SetValue(Grid.RowProperty, 2);

                grid_gamefield.Children.Add(obj.ObjectBorder);
                obj.ObjectBorder.SetValue(Grid.ColumnProperty, i);
                obj.ObjectBorder.SetValue(Grid.RowProperty, 2);

                obj.Row = 2;

                switch (i)
                {
                    case 0:
                        vm.TowerLeft.Add(obj);
                        obj.Visibility = Visibility.Visible;
                        obj.Column = 0;
                        break;
                    case 1:
                        vm.TowerCenter.Add(obj);
                        obj.Column = 1;
                        break;
                    case 2:
                        vm.TowerRight.Add(obj);
                        obj.Column = 2;
                        break;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                ShroomObject obj = new ShroomObject();
                obj.Width = 180;
                obj.Height = 60;
                obj.Visibility = Visibility.Hidden;
                obj.Stretch = Stretch.Fill;
                obj.Source = new BitmapImage(new Uri("Resources/shroom_red.jpg", UriKind.Relative));
                obj.ObjectBorder.BorderThickness = new Thickness(3);
                obj.ObjectBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                obj.ObjectBorder.Width = obj.Width;
                obj.ObjectBorder.Height = obj.Height;
                obj.ObjectBorder.Visibility = Visibility.Visible;

                grid_gamefield.Children.Add(obj);
                obj.SetValue(Grid.ColumnProperty, i);
                obj.SetValue(Grid.RowProperty, 3);

                grid_gamefield.Children.Add(obj.ObjectBorder);
                obj.ObjectBorder.SetValue(Grid.ColumnProperty, i);
                obj.ObjectBorder.SetValue(Grid.RowProperty, 3);

                obj.Row = 3;

                switch (i)
                {
                    case 0:
                        vm.TowerLeft.Add(obj);
                        obj.Visibility = Visibility.Visible;
                        obj.Column = 0;
                        break;
                    case 1:
                        vm.TowerCenter.Add(obj);
                        obj.Column = 1;
                        break;
                    case 2:
                        vm.TowerRight.Add(obj);
                        obj.Column = 2;
                        break;
                }
            }
        }
    }
}
