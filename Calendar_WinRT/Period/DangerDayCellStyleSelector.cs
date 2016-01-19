using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Input;
using Telerik.UI.Xaml.Controls.Input.Calendar;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Collections;
using Windows.UI.Xaml.Media.Imaging;

namespace Calendar_WinRT.Period {
    class DangerDayCellStyleSelector:CalendarCellStyleSelector {

        //private Logger logger = new Logger();
        private List<CalendarCellStyle> listCellStyle = new List<CalendarCellStyle>();
        private List<DateTime> listBloodyDay ;
        private List<String> allDangerDates { get; set; }
        private CalendarCellStyle bloodyCellStyle;

        public DangerDayCellStyleSelector(List<CalendarDateRange> danger, ObservableCollection<DateTime> blood) {
            listBloodyDay = new List<DateTime>(blood);
            createDangerCellStyle(danger);
            bloodyCellStyle = createBloodyCellStyle();
        }

        private CalendarCellStyle createBloodyCellStyle() {
            CalendarCellStyle c = new CalendarCellStyle();
            c.DecorationStyle = new Style(typeof(Border));
            c.ContentStyle = new Style(typeof(TextBlock));

            ImageBrush background = new ImageBrush();
            background.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/blood-drop-icon.png"));
            c.DecorationStyle.Setters.Add(new Setter(Border.BackgroundProperty, background));

            c.ContentStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 35));
            c.ContentStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            c.ContentStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            return c;
        }

        private void createDangerCellStyle(List<CalendarDateRange> danger) {
            allDangerDates = new List<string>();
            foreach(CalendarDateRange range in danger) {
                allDangerDates.AddRange(range.Select(e=> e.ToString("MMMM dd, yyyy")));
            }

            for (int i = 0; i < 7; i++) {
                string colorCode;
                int thickness;
                switch (i) {
                    case 0:
                    case 6:
                        colorCode = "229,115,115"; //229,115,115
                        thickness = 40;
                        break;
                    case 1:
                    case 5:
                        colorCode = "244,67,54"; //244,67,54
                        thickness = 30;
                        break;
                    case 2:
                    case 4:
                        colorCode = "211,47,47"; //211,47,47
                        thickness = 20;
                        break;
                    default:
                        colorCode = "183,28,28"; //183,28,28
                        thickness = 10;
                        break;
                }
                CalendarCellStyle newStyle = new DangerDayCellStyle(colorCode, thickness).cellStyle;
                listCellStyle.Add(newStyle);
            }
        }

        protected override void SelectStyleCore(CalendarCellStyleContext context, RadCalendar container) {
            if (context.IsBlackout && container.DisplayMode == CalendarDisplayMode.MonthView) {
                int position = allDangerDates.IndexOf(context.Date.ToString("MMMM dd, yyyy")) % 7;
                context.CellStyle = listCellStyle[position];
            } else {
                foreach (DateTime d in listBloodyDay) {
                    if (context.Date.ToString("MMMM dd, yyyy").Equals(d.ToString("MMMM dd, yyyy"))) {
                        context.CellStyle = bloodyCellStyle;
                    }
                }
            }
        }
    }
}