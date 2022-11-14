using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.ViewModel
{
    public class NetworkViewViewModel : BindableBase
    {
        public static int monitor = 0;
        private ListView lv;
        public BindingList<Parking> Items { get; set; }
        public MyICommand<ListView> MLBUCommand { get; set; }
        public MyICommand<Parking> SCCommand { get; set; }
        public MyICommand<Canvas> DCommand { get; set; }
        public MyICommand<Canvas> FreeCommand { get; set; }
        public MyICommand<Canvas> LCommand { get; set; }
        public MyICommand<ListView> LLWCommand { get; set; }

        Dictionary<int, Parking> temp = new Dictionary<int, Parking>();
        public static Parking draggedItem = null;
        private bool dragging = false;
        private static bool exists = false;
        private int selectedIndex = 0;
        int count = 0;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public NetworkViewViewModel()
        {
            foreach (Parking ob in DataBase.Parkings)
            {
                temp.Add(ob.Id, ob);
                count++;
            }
            Items = new BindingList<Parking>();
            foreach (var item in DataBase.Parkings)
            {
                exists = false;
                foreach (var ex in DataBase.CanvasObjects.Values)
                {
                    if (ex.Id == item.Id)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists == false)
                    Items.Add(new Parking(item));
            }
            MLBUCommand = new MyICommand<ListView>(OnMLBU);
            SCCommand = new MyICommand<Parking>(SelectionChange);
            DCommand = new MyICommand<Canvas>(OnDrop);
            FreeCommand = new MyICommand<Canvas>(OnFree);
            LCommand = new MyICommand<Canvas>(OnLoad);
            LLWCommand = new MyICommand<ListView>(OnLLW);
        }

        public void OnLLW(ListView listview)
        {
            lv = listview;
        }

        public void OnLoad(Canvas c)
        {
            if (DataBase.CanvasObjects.ContainsKey(c.Name))
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(DataBase.CanvasObjects[c.Name].Type.Slika);
                logo.EndInit();
                c.Background = new ImageBrush(logo);
                c.Resources.Add("taken", true);
                monitor++;
                CheckValue(c);
            }
        }

        public void OnFree(Canvas c)
        {
            try
            {
                if (c.Resources["taken"] != null)
                {
                    c.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;

                    foreach (var item in DataBase.Parkings)
                    {
                        if (!Items.Contains(item) && DataBase.CanvasObjects[c.Name].Id == item.Id)
                        {
                            Items.Add(new Parking(item));
                        }
                    }
                    c.Resources.Remove("taken");
                    DataBase.CanvasObjects.Remove(c.Name);
                }
                monitor--;
            }
            catch (Exception) { }
        }

        public void OnDrop(Canvas c)
        {
            if (draggedItem != null)
            {
                if (c.Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(draggedItem.Type.Slika);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    DataBase.CanvasObjects[c.Name] = draggedItem;
                    c.Resources.Add("taken", true);
                    Items.Remove(Items.Single(x => x.Id == draggedItem.Id));
                    SelectedIndex = 1;
                    monitor++;
                    CheckValue(c);
                }
                dragging = false;
            }
        }

        public void OnMLBU(ListView lw)
        {
            draggedItem = null;
            lw.SelectedItem = null;
            dragging = false;
        }

        public void SelectionChange(Parking o)
        {
            if (!dragging)
            {
                dragging = true;
                draggedItem = new Parking(o);
                DragDrop.DoDragDrop(lv, draggedItem, DragDropEffects.Move);
            }
        }

        private void CheckValue(Canvas c)
        {

            foreach (Parking ob in DataBase.Parkings)
            {
                temp[ob.Id] = ob;
            }
            Task.Delay(1000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (DataBase.CanvasObjects.Count != 0)
                    {
                        if (DataBase.CanvasObjects.ContainsKey(c.Name))
                        {
                            if (temp[DataBase.CanvasObjects[c.Name].Id].Value > 90)
                            {
                                ((Border)c.Children[0]).BorderBrush = Brushes.Red;
                            }
                            else
                            {
                                ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;
                            }
                        }
                        else
                        {
                            ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;
                        }
                    }
                });
                CheckValue(c);
            });

        }
    }
}
