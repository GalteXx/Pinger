using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Ping pinger = new Ping();
        DispatcherTimer timer = new DispatcherTimer();
        System.Windows.Forms.NotifyIcon trayIcon = new System.Windows.Forms.NotifyIcon();
        


        public MainWindow()
        {
            InitializeComponent();
            Config.Content = new string[2];
            this.Topmost = true;


            Config.setup();
            Cmd.Initialize();
            FWBSetup();
            

            //tray icon setup
            trayIcon.Icon = new System.Drawing.Icon("ico.ico");
            trayIcon.Visible = true;
            trayIcon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };


            //timer setup
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Tick;
            timer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            //pinging
            PingReply localPing = pinger.Send(Config.Content[0]);
            PingReply GooglePing = pinger.Send(Config.Content[1]);
            //updating displayed data
            label1.Content = localPing.RoundtripTime;
            label2.Content = GooglePing.RoundtripTime;
            trayIcon.Text = label1.Content + " | " + label2.Content;
        }



        //closing application
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Cmd.Exit();
            Application.Current.Shutdown();
        }

        //minimising application
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        //dragging area
        private void dragArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void FWBSetup()
        {
            if (Config.Content[2] != "false")
            {
                //some basic setup
                this.Height = 74;
                Button FWButton = new Button();
                mainGrid.Children.Add(FWButton);
                FWButton.Height = 14.527;
                FWButton.Width = 37.428;
                FWButton.BorderBrush = Brushes.Transparent;
                Thickness temp = new Thickness();
                temp.Top = 59.428;
                temp.Left = 0;
                FWButton.Margin = temp;


                if (Config.Content[3] == "yes")
                {
                    Config.FWRon = true;
                    FWButton.Background = Brushes.Green;
                }
                else
                {
                    Config.FWRon = false;
                    FWButton.Background = Brushes.Red;
                    
                }

                //applying default rules
                Cmd.Execute("netsh advfirewall firewall set rule name="+ Config.Content[2] + " new enable=" + Config.Content[3]);


                FWButton.Click +=
                    delegate (object sender, RoutedEventArgs e)
                    {
                        if (Config.FWRon == true)
                        {
                            Cmd.Execute("netsh advfirewall firewall set rule name=" + Config.Content[2] + " new enable=no");
                            FWButton.Background = Brushes.Red;
                            Config.FWRon = false;
                        }
                        else
                        {
                            Cmd.Execute("netsh advfirewall firewall set rule name=" + Config.Content[2] + " new enable=yes");
                            FWButton.Background = Brushes.Green;
                            Config.FWRon = true;
                        }
                        

                    };
            }
        }



            
            
        
    }
}
