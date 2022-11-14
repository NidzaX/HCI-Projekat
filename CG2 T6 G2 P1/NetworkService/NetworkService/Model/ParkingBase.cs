using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class DataBase
    {
        public static ObservableCollection<Parking> Parkings { get; set; } = new ObservableCollection<Parking>();
        public static Dictionary<string, Parking> CanvasObjects { get; set; } = new Dictionary<string, Parking>();
    }

    public class Parking : BindableBase
    {
        private int id;
        private string name;
        private Type type = new Type();
        private double value;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public double Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
                int a = ViewModel.DataChartViewModel.ParkingChoice;
                if (a == this.id)
                {
                    if (value <= 90)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina1 = ViewModel.DataChartViewModel.CalculateElementHeight(value, this.id);
                        ViewModel.DataChartViewModel.ElementHeights.Boja1 = "Green";
                    }
                    else if (value > 90 || value != -1)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Visina1 = ViewModel.DataChartViewModel.CalculateElementHeight(value, this.id);
                        ViewModel.DataChartViewModel.ElementHeights.Boja1 = "Red";
                    }
                }
            }
        }



        public Type Type
        {
            get { return type; }
            set
            {
                type.Name = value.Name;
                type.Slika = value.Slika;
            }
        }



        public Parking()
        {
        }
        public Parking(Parking s)
        {
            Id = s.Id;
            Name = s.Name;
            Type = s.type;
            Value = s.Value;
        }
    }
}
