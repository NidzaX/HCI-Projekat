using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string path;

        private int id;
        private double value;
        private bool file;
        public MainWindow()
        {
            path = Environment.CurrentDirectory + @"\Log.txt";
            File.WriteAllText(path, String.Empty);
            InitializeComponent();
            createListener(); //Povezivanje sa serverskom aplikacijom
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            int c = ViewModel.NetworkViewViewModel.monitor;
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(DataBase.Parkings.Count().ToString());
                            stream.Write(data, 0, data.Length);
                            file = false;
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Objekat_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            string[] split = incomming.Split('_', ':');
                            id = Int32.Parse(split[1]);
                            value = Double.Parse(split[2]);
                            //MyCollection.Any(p => p.name == "bob" && p.Checked)
                            if (DataBase.Parkings.Count() > 0 && (DataBase.Parkings.Count() > id))
                            {
                                DataBase.Parkings[id].Value = value;
                                WriteLog(split, DataBase.Parkings[id].Id);
                            }
                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        private void WriteLog(string[] split, int id)
        {
            if (!file)
            {
                StreamWriter writer;
                File.AppendAllText(@"Log.txt", $"Parking: {id}\t|Popunjenost: {int.Parse(split[2])}\t|Time: {DateTime.Now}" + Environment.NewLine);
                file = true;
            }
            else
            {
                StreamWriter writer;
                File.AppendAllText(@"Log.txt", $"Parking:{id}\t|Popunjenost: {int.Parse(split[2])}\t|Time: {DateTime.Now}" + Environment.NewLine);
            }
            file = true;
        }
    }
}
