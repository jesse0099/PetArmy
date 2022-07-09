using Resx;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public class AdminRequestStatusTranslator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            switch (status)
            {
                case Commons.AdminRequestTreatedState: {
                    return AppResources.Treated;
                }
                case Commons.AdminRequestPendingState: {
                    return AppResources.Pending;
                }
                case Commons.AdminRequestRejectedState: {
                    return AppResources.Rejected;
                }
                default:{
                    return "undefined";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
