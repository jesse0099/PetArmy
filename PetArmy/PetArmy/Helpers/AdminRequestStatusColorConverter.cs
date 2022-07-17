using Resx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class AdminRequestStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Application.Current.Resources.TryGetValue("CustomOrange", out object orange);
            Application.Current.Resources.TryGetValue("CustomYellow", out object yellow);
            Application.Current.Resources.TryGetValue("CustomPink", out object pink);
            // Treated
            Application.Current.Resources.TryGetValue("TealGreen", out object teal_green);
            Application.Current.Resources.TryGetValue("TealGreenDark", out object teal_green_dark);

            var status = ((string)value);
            return status switch
            {
                Commons.AdminRequestTreatedState => Application.Current.UserAppTheme == OSAppTheme.Dark ? teal_green_dark : teal_green,
                Commons.AdminRequestPendingState => ((Color)yellow),
                Commons.AdminRequestProcessingState => ((Color)orange),
                Commons.AdminRequestRejectedState => ((Color)pink),
                _ => Color.Black,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
