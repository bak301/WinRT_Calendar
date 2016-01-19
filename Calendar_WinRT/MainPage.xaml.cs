using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Calendar_WinRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void toPeriod_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(Calendar_WinRT.Period.PeriodPage));
        }

        private void toXoSo_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(XoSoKienThiet.Xoso));
        }

        private void toMusic_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(MediaPlayer.MediaPlayer));
        }
    }
}
