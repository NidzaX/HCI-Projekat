using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.ViewModel
{
    public class DataChartViewModel : BindableBase
    {
        public Dictionary<int, Parking> ComboBoxDataParking { get; set; }   // za dodavanje
        public static GraphUpdater ElementHeights { get; set; } = new GraphUpdater();
        private static int reactorChoice;
        public static MyICommand<string> PlotCommand { get; set; }


        public DataChartViewModel()
        {
            ComboBoxDataParking = new Dictionary<int, Parking>();
            PlotCommand = new MyICommand<string>(OnPlot);
            foreach (Parking r in DataBase.Parkings)
            {
                if (!ComboBoxDataParking.ContainsKey(r.Id))
                {
                    ComboBoxDataParking.Add(r.Id, r);
                }
            }
        }

        public int idInt;

        private void OnPlot(string s)
        {
            bool postoji = false;
            Parking r = new Parking();
            string[] p = s.Split('(', ')', ',');

            if (Int32.TryParse(p[1], out idInt))
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
                MessageBox.Show("Parametar mora biti broj", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (postoji)
            {
                ParkingChoice = r.Id;
            }
            else
            {
                MessageBox.Show("Parking sa tim id-om ne postoji", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        public static int ParkingChoice
        {
            get { return reactorChoice; }
            set
            {
                reactorChoice = value;
                List<int> vrijednost = new List<int>();
                List<DateTime> dates = new List<DateTime>();
                string[] lines = File.ReadAllLines("Log.txt");
                List<String> l = lines.ToList();
                l.Reverse();
                foreach (string s in l)
                {
                    DateTime dt = DateTime.Parse($"{s.Split(':', '|')[5]}:{s.Split(':', '|')[6]}:{s.Split(':', '|')[7]}".Trim());
                    int id = int.Parse(s.Split(':', '|')[1]);
                    if (id == ParkingChoice)
                    {
                        vrijednost.Add(int.Parse(s.Split(':', '|')[3]));
                        dates.Add(dt);
                    }

                    if (dates.Count == 5)
                        break;
                }
                int duzina = vrijednost.Count();
                int a = 3;

                if (duzina >= 1)
                {
                    if (vrijednost[0] <=90)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina1 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[0], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja1 = "Green";
                    }
                    else if (vrijednost[0] > 90 || vrijednost[0] != -1)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina1 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[0], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja1 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Visina1 = -1;
                }

                if (duzina >= 2)
                {
                    if (vrijednost[1] <=90)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina2 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[1], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja2 = "Green";
                    }
                    else if (vrijednost[1] > 90 || vrijednost[1] != -1)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina2 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[1], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja2 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Visina2 = -1;
                }

                if (duzina >= 3)
                {
                    if (vrijednost[2] <=90)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina3 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[2], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja3 = "Green";
                    }
                    else if (vrijednost[2] > 90 || vrijednost[2] != -1)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina3 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[2], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja3 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Visina3 = -1;
                }

                if (duzina >= 4)
                {
                    if (vrijednost[3] <=90)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina4 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[3], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja4 = "Green";
                    }
                    else if (vrijednost[3] > 90 || vrijednost[3] != -1)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina4 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[2], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja4 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Visina4 = -1;
                }

                if (duzina >= 5)
                {
                    if (vrijednost[4] <=90)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina5 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[4], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja5 = "Green";
                    }
                    else if (vrijednost[4] >90 || vrijednost[4] != -1)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina5 = ViewModel.DataChartViewModel.CalculateElementHeight(vrijednost[4], a);
                        ViewModel.DataChartViewModel.ElementHeights.Boja5 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Visina5 = -1;
                }
            }
        }

        public static double CalculateElementHeight(double value, int a)
        {
            return value *2.2;
        }
    }
}
