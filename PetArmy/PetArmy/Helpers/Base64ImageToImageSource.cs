using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class Base64ImageToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return null;
                if ((string)value == string.Empty) return null;

                string base_source = (string)value;
                byte[] bytes = System.Convert.FromBase64String(base_source);
                var image_stream = new MemoryStream(bytes);
                return ImageSource.FromStream(() => image_stream);
            }
            catch (Exception)
            {

                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
