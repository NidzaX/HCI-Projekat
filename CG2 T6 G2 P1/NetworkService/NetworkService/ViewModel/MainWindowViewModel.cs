using System.Windows.Controls;
using NetworkService.Model;
using System.Windows;

namespace NetworkService.ViewModel
{
    class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        private NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        private NetworkViewViewModel networkViewViewModel = new NetworkViewViewModel();
        private DataChartViewModel dataChartViewModel = new DataChartViewModel();
        private BindableBase currentViewModel;
        public int monitor = 0;

        private string help = "Lista komandi: \n\n" +
            "- dodaj(id,ime,tip) - dodaj entitet  " +
            "tip- otvoreni/zatvoreni\n" +
            "- obrisi(id) - obrisi entitet\n" +
            "- iscrtaj(id) - iscratj entitet sa datim id-om\n" +
            "- preatrazi(ime/tip,vrednost) - pretrazi po imenu ili tipu\n" +
            "- viewView - preabci na View view\n" +
            "- chartView - preabci na Chart view\n" +
            "- viewData - prebaci na Netw Data view\n" +
            "- undo - korak unazad";

        private bool ind = false;
        public MyICommand<TextBox> terminal { get; set; }
        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
            CurrentViewModel = networkDataViewModel;
            terminal = new MyICommand<TextBox>(Terminal);
        }


        public void Terminal(TextBox tb)
        {
            string[] p;
            string f = "";
            if (ind == true)
            {
                f = tb.Text.Substring(help.Length);
                p = f.Split('(', ')', ',');
                ind = false;
            }
            else
            {
                p = tb.Text.Split('(', ')', ',');
                f = tb.Text;
            }

            if (p[0] == "pomoc" && p.Length == 1)
            {
                ind = true;
                tb.Text = help;
                tb.FontSize = 16;

            }
            else if (p[0] == "dodaj")
            {
                NetworkDataViewModel.AddCommand.Execute(f);
                tb.Text = "";
            }
            else if (p[0] == "obrisi")
            {
                NetworkDataViewModel.DeleteCommand.Execute(f);
                tb.Text = "";

            }
            else if (p[0] == "chartView")
            {
                CurrentViewModel = dataChartViewModel;
                tb.Text = "";
            }
            else if (p[0] == "viewView")
            {
                CurrentViewModel = networkViewViewModel;
                tb.Text = "";
            }
            else if (p[0] == "viewData")
            {
                CurrentViewModel = networkDataViewModel;
                tb.Text = "";
            }
            else if (p[0] == "undo")
            {
                NetworkDataViewModel.UndoCommand.Execute(f);
                tb.Text = "";
            }
            else if (p[0] == "iscrtaj")
            {
                CurrentViewModel = dataChartViewModel;
                DataChartViewModel.PlotCommand.Execute(f);
                tb.Text = "";
            }
            else if (p[0] == "preatrazi")
            {
                NetworkDataViewModel.SearchCommandConsole.Execute(f);
                tb.Text = "";
            }
            else
            {
                MessageBox.Show("Nepoznata komanda", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                tb.Text = "";
            }
        }






        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "NetworkData":
                    CurrentViewModel = networkDataViewModel;
                    break;
                case "NetworkView":
                    CurrentViewModel = networkViewViewModel;
                    break;

                case "DataChart":
                    CurrentViewModel = dataChartViewModel;
                    break;
            }
        }
    }
}
