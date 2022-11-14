using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class GraphUpdater : BindableBase
    {
        private double firstBindingPoint;
        public double Visina1
        {
            get { return firstBindingPoint; }
            set
            {
                Visina2 = firstBindingPoint;
                firstBindingPoint = value;
                OnPropertyChanged("Visina1");
            }
        }

        private string firstBindingPoint2;
        public string Boja1
        {
            get { return firstBindingPoint2; }
            set
            {
                Boja2 = firstBindingPoint2;
                firstBindingPoint2 = value;
                OnPropertyChanged("Boja1");
            }
        }

        private double secondBindingPoint;
        public double Visina2
        {
            get { return secondBindingPoint; }
            set
            {
                Visina3 = secondBindingPoint;
                secondBindingPoint = value;
                OnPropertyChanged("Visina2");
            }
        }


        private string secondBindingPoint2;
        public string Boja2
        {
            get { return secondBindingPoint2; }
            set
            {
                Boja3 = secondBindingPoint2;
                secondBindingPoint2 = value; ;
                OnPropertyChanged("Boja2");
            }
        }

        private double thirdBindingPoint;
        public double Visina3
        {
            get { return thirdBindingPoint; }
            set
            {
                Visina4 = thirdBindingPoint;
                thirdBindingPoint = value;
                OnPropertyChanged("Visina3");
            }
        }

        private string thirdBindingPoint2;
        public string Boja3
        {
            get { return thirdBindingPoint2; }
            set
            {
                Boja4 = thirdBindingPoint2;
                thirdBindingPoint2 = value;
                OnPropertyChanged("Boja3");
            }
        }

        private double fourthBindingPoint;
        public double Visina4
        {
            get { return fourthBindingPoint; }
            set
            {
                Visina5 = fourthBindingPoint;
                fourthBindingPoint = value;
                OnPropertyChanged("Visina4");
            }
        }


        private string fourthBindingPoint2;
        public string Boja4
        {
            get { return fourthBindingPoint2; }
            set
            {
                Boja5 = fourthBindingPoint2;
                fourthBindingPoint2 = value;
                OnPropertyChanged("Boja4");
            }
        }

        private double fifthBindingPoint;
        public double Visina5
        {
            get { return fifthBindingPoint; }
            set
            {
                fifthBindingPoint = value;
                OnPropertyChanged("Visina5");
            }
        }

        private string fifthBindingPoint2;
        public string Boja5
        {
            get { return fifthBindingPoint2; }
            set
            {
                fifthBindingPoint2 = value;
                OnPropertyChanged("Boja5");
            }
        }
    }
}
