using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class AdminRequestActionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            switch (status)
            {
                case Commons.AdminRequestPendingState: {
                    return true;
                }
                default: {
                    return false;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
