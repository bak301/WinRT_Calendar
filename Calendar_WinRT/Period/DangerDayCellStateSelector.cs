using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Input;
using Telerik.UI.Xaml.Controls.Input.Calendar;
using Windows.Storage;

namespace Calendar_WinRT.Period {
    class DangerDayCellStateSelector : CalendarCellStateSelector {
        public List<CalendarDateRange> list { get; set; }

        public DangerDayCellStateSelector(List<CalendarDateRange> list) {
            this.list = list;
        }

        protected override void SelectStateCore(CalendarCellStateContext context, RadCalendar container) {
            for (int i = 0; i < list.Count; i++) {
                if (container.DisplayMode == CalendarDisplayMode.MonthView) {
                    if (context.Date.ToString("MMMM dd, yyyy").Equals(list[i].StartDate.ToString("MMMM dd, yyyy"))) {
                        context.IsBlackout = true;
                    } else if (context.Date > list[i].StartDate && context.Date <= list[i].EndDate) {
                        context.IsBlackout = true;  
                    }
                }
            }
        }
    }
}
