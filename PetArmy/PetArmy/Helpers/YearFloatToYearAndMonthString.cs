using Resx;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class YearFloatToYearAndMonthString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var parsed_value = (float)value;
            var whole_part = Math.Truncate(parsed_value);
            string years = System.Convert.ToString(whole_part);
            string months = System.Convert.ToString(Math.Truncate((parsed_value - whole_part) / 0.0833));
            return $"{AppResources.Years} : {years}    {AppResources.Months} : {months}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
