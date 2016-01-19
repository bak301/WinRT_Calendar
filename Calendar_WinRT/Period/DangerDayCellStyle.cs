using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Calendar_WinRT.Period {
    class DangerDayCellStyle {
        public CalendarCellStyle cellStyle { get; set; }

        public DangerDayCellStyle(string color, int thickness) {
            cellStyle = new CalendarCellStyle();
            Style blackOutDecoration = new Style(typeof(Border));
            Style blackOutContent = new Style(typeof(TextBlock));

            char[] delimiter = { ',' };
            byte[] rgbColor = color.Split(delimiter).Select(e=> Byte.Parse(e)).ToArray();
            Color red = Color.FromArgb(255, rgbColor[0], rgbColor[1], rgbColor[2]);

            blackOutDecoration.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(red)));
            blackOutDecoration.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(60)));
            blackOutDecoration.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(thickness)));
            cellStyle.DecorationStyle = blackOutDecoration;

            blackOutContent.Setters.Add(new Setter(TextBlock.FontSizeProperty, 35));
            blackOutContent.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            blackOutContent.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            cellStyle.ContentStyle = blackOutContent;
        }
    }
}
