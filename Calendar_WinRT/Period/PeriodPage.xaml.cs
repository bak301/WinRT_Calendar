
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Telerik.UI.Xaml.Controls.Input;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Calendar_WinRT.Period
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PeriodPage : Page
    {
        GirlPeriod period;

        public PeriodPage()
        {
            this.InitializeComponent();
            period = new GirlPeriod();
            firstDayPicker.Value = Convert.ToDateTime("01/18/2016");
            nextDayPicker.Value = Convert.ToDateTime("02/18/2016");
        }

        private void btnInitializeGirlPeriod_Click(object sender, RoutedEventArgs e) {

            if (firstDayPicker.Visibility == Visibility.Visible) {
                DateTime first = (DateTime) firstDayPicker.Value;
                period.listBloodDay.Add(first);
                firstDayPicker.Visibility = Visibility.Collapsed;
            }

            DateTime next = (DateTime) nextDayPicker.Value;
            period.listBloodDay.Add(next);

            CalendarDateRange lastRange = period.listDangerDate.Last();
            txtFirstDay.Text = lastRange.StartDate.ToString() + " || " + lastRange.EndDate.ToString();
            

            mainCalendar.DisplayMode = CalendarDisplayMode.MonthView;
            mainCalendar.MoveToDate(lastRange.StartDate.AddTicks((lastRange.EndDate.Ticks - lastRange.StartDate.Ticks) / 2));
            mainCalendar.CellStateSelector = new DangerDayCellStateSelector(period.listDangerDate);
            mainCalendar.CellStyleSelector = new DangerDayCellStyleSelector(period.listDangerDate, period.listBloodDay);
        }

        private void back_Click(object sender, RoutedEventArgs e) {
            if (this.Frame != null) {
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
