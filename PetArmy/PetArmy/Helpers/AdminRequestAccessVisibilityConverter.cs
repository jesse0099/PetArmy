using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class AdminRequestAccessVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            switch (status){
                case Commons.AdminRequestTreatedState : {
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