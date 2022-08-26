using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class PetStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            App.Current.Resources.TryGetValue("CustomOrange", out object nullColor);
            App.Current.Resources.TryGetValue("CustomPink", out object addoptedColor);
            App.Current.Resources.TryGetValue("TealGreen", out object noAddoptedColor);
            if (value == null)
                return  (Color)nullColor;

            //Adoptado
            if((bool)value)
                return (Color)addoptedColor;
            //No Adoptado
            return (Color)noAddoptedColor; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
