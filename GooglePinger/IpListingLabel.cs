using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GooglePinger
{
    class IpListingLabel : Label
    {
        
        public bool hasActualIp { get; set; } = false;

        public void checkd(object sender, MouseButtonEventArgs e)
        {
            if(hasActualIp)
                Config.Content[0] = this.Content.ToString();
        }
            

        //System.Windows.Controls.Grid
        public IpListingLabel(int i, string text, Grid stuffMainGrid, bool ipInIt)
        {
            // just setting up some default params
            System.Windows.Thickness temp = new System.Windows.Thickness();
            temp.Left = 10;
            temp.Top = 70 + i * 30;
            Margin = temp;
            Height = 30;
            Width = 683;
            MinHeight = 30;
            MinWidth = 683;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Visibility = System.Windows.Visibility.Visible;
            Foreground = System.Windows.Media.Brushes.White;

            Content = text;
            hasActualIp = ipInIt;

            this.MouseDown += checkd;

            Grid.SetColumn(this, 0);
            Grid.SetRow(this, 0);
            stuffMainGrid.Children.Add(this);
        }
    }
}
