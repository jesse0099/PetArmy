using Resx;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class CreateAccountButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var casted_value = (bool)value;
            if (casted_value)
                return AppResources.RequestAccount;
            else
                return AppResources.CreateAccountKey;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}