using Resx;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class WeightFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return $"{AppResources.WeightKey} : {Math.Truncate((float)value)} Kg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
