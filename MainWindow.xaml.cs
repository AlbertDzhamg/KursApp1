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
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;

namespace Курсовой_Будякова
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        struct server 
        {
            public string host;
            public string login;
            public string pwd;
            public string db;
        }
        public Window1()
        {
            InitializeComponent();
            if(File.Exists(Directory.GetCurrentDirectory()+"\\conf.json"))
            {
               
                    var _jsonconf = File.ReadAllText(Directory.GetCurrentDirectory() + "\\conf.json");
                if (_jsonconf != null)
                {
                    try
                    {
                        server serv_conf = JsonConvert.DeserializeObject<server>(_jsonconf);
                    //"Host=localhost;Username=postgres;Password=217690zx;Database=myProject";
                    App.sc= "Host="+serv_conf.host + ";Username=" + serv_conf.login + ";Password="+serv_conf.pwd + ";Database=" + serv_conf.db+";";}
                    catch 
                    {
                        MessageBox.Show("Error read config!");
                    }
                }
                else
                {
                    server serv_conf = new server { db = "myDatabase", host = "myHost", login="myLogin", pwd="myPassword"};
                    var jsonRole = JsonConvert.SerializeObject(serv_conf);
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\conf.json", false))
                        {
                            writer.Write(jsonRole);
                        }
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show("Ошибка записи json файла конфигурации/n" + e.Message);
                    }
                }
               
            }
            else 
            {
                server serv_conf = new server { db = "myDatabase", host = "myHost", login = "myLogin", pwd = "myPassword" };
                var jsonRole = JsonConvert.SerializeObject(serv_conf);
                try
                {
                    using (FileStream fs = File.Create(Directory.GetCurrentDirectory() + "\\conf.json"))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(jsonRole);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
                catch (IOException e)
                {
                    MessageBox.Show("Ошибка записи json файла конфигурации/n" + e.Message);
                }
            }

        }
        private void Countrys_Click(object sender, RoutedEventArgs e)
        {
           View.CountryView country = new View.CountryView();
            country.Show();
        }

        private void Citys_Click(object sender, RoutedEventArgs e)
        {
            View.CityView Citys = new View.CityView();
            Citys.Show();
        }

        private void Regions_Click(object sender, RoutedEventArgs e)
        {
            View.RegionView country = new View.RegionView();
            country.Show();
        }
        private void Address_Click(object sender, RoutedEventArgs e)
        {
            View.AddressView country = new View.AddressView();
            country.Show();
        }
    }
}
