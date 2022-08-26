using Resx;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    internal class PetStatusStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            //Adoptado
            if ((bool)value)
                return AppResources.Adopted;
            //No Adoptado - Disponible
            return AppResources.Available;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
