using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Windows.Threading;

namespace GooglePinger
{
    /// <summary>
    /// Логика взаимодействия для Stuff.xaml
    /// </summary>
    public partial class Stuff : Window
    {
        public Stuff(NetworkInterface[] cabels)
        {
            InitializeComponent();
            chooseIP(cabels);
        }

        private void chooseIP(NetworkInterface[] cabels)
        {
            //prepare for something I wouldnt do even in cpp
            string lastIP = "";
            int i = 0;
            //checking all Gateways connected to PC
            List<IpListingLabel> ipList = new List<IpListingLabel>();
            foreach (NetworkInterface cabel in cabels)
            {
                
                ipList.Add(new IpListingLabel(i, cabel.Name + " -- " + cabel.Description, this.StuffMainGrid, false));
                i++;


                IPInterfaceProperties props = cabel.GetIPProperties();
                GatewayIPAddressInformationCollection ipCol = props.GatewayAddresses;
                foreach (GatewayIPAddressInformation ipOne in ipCol)
                {

                    ipList.Add(new IpListingLabel(i, ipOne.Address.ToString(), this.StuffMainGrid, true));
                    ipList[i].MouseDown +=
                        delegate
                        {
                            this.Close();
                        };
                    ipList[i].hasActualIp = true;
                    i++;
                    lastIP = ipOne.Address.ToString();
                }
            }
        }

        //offering user to edit config manually
        private void infoLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", System.IO.Directory.GetCurrentDirectory());
        }
    }
}
