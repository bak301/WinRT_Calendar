using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Input;

namespace Calendar_WinRT.Period {
    class GirlPeriod {
        public ObservableCollection<DateTime> listBloodDay { get; set; }
        public List<CalendarDateRange> listDangerDate { get; set; }

        public GirlPeriod() {
            listDangerDate = new List<CalendarDateRange>();
            listBloodDay = new ObservableCollection<DateTime>();
            listBloodDay.CollectionChanged += ArrNgayKinh_CollectionChanged;
        }

        private void ArrNgayKinh_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (listBloodDay.Count > 1) {
                buildDangerWeek();
            }
        }

        private void buildDangerWeek() {
            int lastDay = listBloodDay.Count-1;
            CalendarDateRange dangerRange = getTuanNguyHiem(listBloodDay[lastDay - 1], listBloodDay[lastDay]);
            listDangerDate.Add(dangerRange);
        }

        private CalendarDateRange getTuanNguyHiem(DateTime firstDay, DateTime nextDay) {
            DateTime firstDayOnDangerWeek = firstDay + new TimeSpan((nextDay - firstDay).Ticks / 2) - new TimeSpan(3,0,0,0);
            DateTime lastDayOnDangerWeek = firstDayOnDangerWeek + new TimeSpan(6, 0, 0, 0);
            CalendarDateRange temp = new CalendarDateRange(firstDayOnDangerWeek, lastDayOnDangerWeek);
            return temp;
        }

        //private void predictNextMonth() {
        //    DateTime nextPeriodDay = listBloodDay.Last() + (listBloodDay.Last() - listBloodDay[listBloodDay.Count - 2]);
        //    listDangerDate.Add(getTuanNguyHiem(listBloodDay[listBloodDay.Count - 1], nextPeriodDay));
        //}
    }
}
