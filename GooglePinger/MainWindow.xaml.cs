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



    }
}
