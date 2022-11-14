using NetworkService.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase
    {
        private DataBase dataBase = new DataBase();
        public List<Parking> SerachParkings = new List<Parking>();
        public ObservableCollection<Parking> CopyParkings { get; set; } = new ObservableCollection<Parking>();
        public Dictionary<int, Parking> Reserve { get; set; } = new Dictionary<int, Parking>();
        public List<string> ComboBoxData { get; set; }      //kombo box za pretragu 

        Stack stekAdd = new Stack();
        Stack stekDelete = new Stack();
        Stack bol = new Stack();

        bool addUndo, deleteUndo = false;
        public List<string> ComboBoxDataParking { get; set; }   // za dodavanje
        private NetworkViewViewModel networkViewViewModel = new NetworkViewViewModel();
        private DataChartViewModel dataChartViewModel = new DataChartViewModel();
        public static MyICommand<string> AddCommand { get; set; }
        public static MyICommand<string> SearchCommandConsole { get; set; }
        public static MyICommand AddCommandBtn { get; set; }  //komande za dugmice
        public static MyICommand<string> DeleteCommand { get; set; }
        public static MyICommand DeleteCommandBtn { get; set; }
        public MyICommand SearchCommand { get; set; }
        public MyICommand NameSearchCommand { get; set; }
        public static MyICommand<string> UndoCommand { get; set; }
        public MyICommand TypeSearchCommand { get; set; }
        public ObservableCollection<Parking> Parking3 { get; set; } = new ObservableCollection<Parking>();

        private string idSearch;
        private string typeText;
        private string id;
        private string name;
        private string searchValueText;
        private int nameOrType = -1;

        private int index = -1;
        private string path;
        private int clickSearch = 0;
        private string nameButtonSearch = "Pretrazi";

        public NetworkDataViewModel()
        {
            if (Parking3.Count() > 0)
            {
                Parking3.Clear();
            }
            foreach (var i in DataBase.Parkings)
            {
                Parking3.Add(i);
            }
            ComboBoxData = new List<string>();
            ComboBoxDataParking = new List<string> { "Otvoreni", "Zatvoreni" };
            foreach (Parking r in Parking3)
            {
                if (!Reserve.ContainsKey(r.Id))
                {
                    Reserve.Add(r.Id, r);
                }
            }




            NameOrType = 0;
            UndoCommand = new MyICommand<string>(OnUndo);
            AddCommand = new MyICommand<string>(OnAdd);
            AddCommandBtn = new MyICommand(OnAddBtn, CanAdd);
            DeleteCommand = new MyICommand<string>(OnDelete);
            DeleteCommandBtn = new MyICommand(OnDeleteBtn, CanDelete);
            SearchCommand = new MyICommand(OnSearch, CanSearch);
            NameSearchCommand = new MyICommand(OnName);
            TypeSearchCommand = new MyICommand(OnType);
            SearchCommandConsole = new MyICommand<string>(OnSearchConsole);
        }

        public DataBase DataBase
        { get => dataBase; set => dataBase = value; }



        private void OnDeleteBtn()
        {
            bool valid = true;
            foreach (Parking v in DataBase.CanvasObjects.Values)
            {
                if (v.Name == Parking3[index].Name)
                {
                    valid = false;
                    MessageBox.Show("Parking je na monitornigu", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }

            if (valid)
            {
                int i = Index;
                bol.Push(false);
                stekDelete.Push(DataBase.Parkings[i]);
                DataBase.Parkings.RemoveAt(i);
                Parking3.RemoveAt(i);
            }
        }


        private bool CanAdd()
        {
            if (TypeText != null && Name != null && Name != "")//provera da li je odabran tip reaktora
            {
                return true;
            }
            return false;

        }

        public int NameOrType
        {
            get { return nameOrType; }
            set
            {
                nameOrType = value;
            }
        }


        private void OnAddBtn()
        {
            if (Int32.TryParse(Id, out idInt))
            {
                bool postoji = false;
                if (DataBase.Parkings.Count != 0)
                {
                    foreach (Parking v in DataBase.Parkings)
                    {
                        if (idInt == v.Id)
                        {
                            MessageBox.Show("Parking sa tim id-om vec postoji.", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                            postoji = true;
                        }
                    }
                }

                if (!postoji)
                {
                    string type;
                    if (TypeText == "Otvoreni")
                    {
                        type = "otvoreni";
                    }
                    else
                    {
                        type = "zatvoreni";
                    }
                    Parking i = new Parking();
                    i.Id = idInt;
                    i.Name = Name;
                    i.Type.Name = type;
                    string pathImg = Environment.CurrentDirectory;
                    pathImg = pathImg.Remove(pathImg.Length - 10, 10);
                    pathImg += @"\Images\" + TypeText + ".jpg";
                    i.Type.Slika = pathImg;
                    stekAdd.Push(i);
                    bol.Push(true);
                    Parking3.Add(i);
                    DataBase.Parkings.Add(i);
                    foreach (Parking r in DataBase.Parkings)
                    {
                        if (!Reserve.ContainsKey(r.Id))
                        {
                            Reserve.Add(r.Id, r);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("id mora da bude celobrojna vrednost", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        public string IdSearch
        {
            get { return idSearch; }
            set
            {
                idSearch = value;
                OnPropertyChanged("IdSearch");   //kad properti dobije vrednost vrednost se setuje u xaml i obrnuto         
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
                AddCommandBtn.RaiseCanExecuteChanged();
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                AddCommandBtn.RaiseCanExecuteChanged();
            }
        }
        public string TypeText
        {
            get { return typeText; }
            set
            {
                typeText = value;
                Path = Environment.CurrentDirectory;
                Path = Path.Remove(Path.Length - 10, 10);
                Path += @"\Images\" + value + ".jpg";
                AddCommandBtn.RaiseCanExecuteChanged(); //properti dobio vrednost proveri da li moze da se aktivira button koji je povezan na ovu komandu
            }
        }
        public string SearchValueText
        {
            get { return searchValueText; }
            set
            {
                searchValueText = value;
                OnPropertyChanged("SearchValueText");
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }



        public int ClickSearch
        {
            get { return clickSearch; }
            set
            {
                clickSearch = value;
                if (value == 0)
                    NameButtonSearch = "Pretrazi";
                else
                    NameButtonSearch = "Refresuj";
            }
        }
        public string NameButtonSearch
        {
            get { return nameButtonSearch; }
            set
            {
                nameButtonSearch = value;
                OnPropertyChanged("NameButtonSearch");
            }
        }

        public int idInt;


        private void OnUndo(string a)
        {
            if (bol.Count > 0)
            {
                if ((bool)bol.Peek()) // poslednje je dodato
                {
                    bol.Pop();
                    Parking r = new Parking();
                    r = (Parking)stekAdd.Pop();
                    Parking zaBrisanje = new Parking();
                    int j = -1;
                    if (r != null)  //Zadnje je dodato sada ga brise
                    {
                        if (Parking3.Contains(r))
                        {
                            deleteUndo = true;

                            stekDelete.Push(DataBase.Parkings.Last());
                            DataBase.Parkings.Remove(r);
                            Parking3.Remove(r);
                        }
                    }
                    addUndo = false;
                }
                else
                {
                    bol.Pop();
                    Parking r = new Parking();
                    r = (Parking)stekDelete.Pop();
                    DataBase.Parkings.Add(r);
                    Parking3.Add(r);
                }
            }
        }


        private void OnSearchConsole(string s)
        {
            string[] p = s.Split('(', ')', ',');

            if (p[1] == "type")
            {
                foreach (var item in Parking3)
                {
                    if (item.Type.Name.Contains(p[2]))
                    {
                        SerachParkings.Add(item);
                    }
                }

                Parking3.Clear();
                foreach (Parking v in SerachParkings)
                {
                    Parking3.Add(v);
                }
                SerachParkings.Clear();
                ClickSearch = 1;

            }
            else if (p[1] == "name")
            {
                foreach (var item in Parking3)
                {
                    if (item.Name.Contains(p[2]))
                    {
                        SerachParkings.Add(item);
                    }
                }
                Parking3.Clear();
                foreach (Parking v in SerachParkings)
                {
                    Parking3.Add(v);
                }
                SerachParkings.Clear();
                ClickSearch = 1;
            }
            else
            {
                MessageBox.Show("Nepoznati parametri", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void OnDelete(string s)
        {

            Parking r = new Parking();
            bool postoji = false;
            bool valid = true;
            string[] p = s.Split('(', ')', ',');
            if (Int32.TryParse(p[1], out idInt))
            {
                if (p.Length == 3)
                {
                    foreach (Parking v in DataBase.CanvasObjects.Values)
                    {
                        if (v.Id == idInt)
                        {
                            valid = false;
                            MessageBox.Show("Parking je na monitoringu", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                    }

                    if (valid != false)
                    {
                        if (DataBase.Parkings.Count != 0)
                        {
                            foreach (Parking v in DataBase.Parkings)
                            {
                                if (idInt == v.Id)
                                {
                                    postoji = true;
                                    r = v;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Baza podataka je prazna", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        if (postoji)
                        {
                            bol.Push(false);
                            stekDelete.Push(r);
                            DataBase.Parkings.Remove(r);
                            Parking3.Remove(r);

                        }
                        else
                        {
                            MessageBox.Show("Parking sa tim id-eom ne postoji.", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Nepoznata komanda", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private void OnAdd(string s)
        {
            string[] p = s.Split('(', ')', ',');

            if (p.Length == 5)
            {
                if (Int32.TryParse(p[1], out idInt))
                {
                    bool postoji = false;
                    if (DataBase.Parkings.Count != 0)
                    {
                        foreach (Parking v in DataBase.Parkings)
                        {
                            if (idInt == v.Id)
                            {
                                MessageBox.Show("Parking with that id already exists.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                                postoji = true;
                            }
                        }
                    }

                    if (!postoji)
                    {
                        if (p[3] == "zatvoreni" || p[3] == "otvoreni")
                        {
                            string type;
                            if (p[3] == "otvoreni")
                            {
                                type = "otvoreni";
                            }
                            else
                            {
                                type = "zatvoreni";
                            }
                            Parking i = new Parking();
                            i.Id = idInt;
                            i.Name = p[2];
                            i.Type.Name = type;
                            string pathImg = Environment.CurrentDirectory;
                            pathImg = pathImg.Remove(pathImg.Length - 10, 10);
                            pathImg += @"\Images\" + type + ".jpg";
                            i.Type.Slika = pathImg;
                            stekAdd.Push(i);
                            bol.Push(true);
                            Parking3.Add(i);
                            DataBase.Parkings.Add(i);
                            foreach (Parking r in DataBase.Parkings)
                            {
                                if (!Reserve.ContainsKey(r.Id))
                                {
                                    Reserve.Add(r.Id, r);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unknown type", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Id must be a INT32 number", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Unknown command", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void OnName()
        {
            NameOrType = 0;
        }
        public void OnType()
        {
            NameOrType = 1;
        }

        private bool CanSearch()
        {
            if (SearchValueText != null && SearchValueText != "")
                return true;
            return false;
        }

        private void OnSearch()
        {
            if (ClickSearch == 0)    //1 kad moze undo inace 0
            {
                foreach (Parking r in Parking3)
                {
                    CopyParkings.Add(r);
                }

                if (NameOrType == 0) // name
                {
                    foreach (var item in Parking3)
                    {
                        if (item.Name.Contains(SearchValueText))
                        {
                            SerachParkings.Add(item);
                        }
                    }
                }
                else if (NameOrType == 1) // type
                {
                    foreach (var item in Parking3)
                    {
                        if (item.Type.Name.Contains(SearchValueText))
                        {
                            SerachParkings.Add(item);
                        }
                    }

                }
                Parking3.Clear();
                foreach (Parking v in SerachParkings)
                {
                    Parking3.Add(v);
                }
                SerachParkings.Clear();
                ClickSearch = 1;

            }
            else
            {
                Parking3.Clear();
                foreach (Parking v in Reserve.Values)
                {
                    Parking3.Add(v);
                }
                CopyParkings.Clear();
                ClickSearch = 0;
            }
        }
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                DeleteCommandBtn.RaiseCanExecuteChanged();
            }
        }

        private bool CanDelete()
        {
            return index >= 0;
        }

    }
}