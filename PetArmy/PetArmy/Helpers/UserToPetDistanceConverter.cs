using PetArmy.Models;
using PetArmy.ViewModels;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace PetArmy.Helpers
{
    public class UserToPetDistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;
            //Get Current User Location 
            var current_position = EditShelterViewModel.GetCurrentPosition().Result;

            //Cast value to ubicaciones_refugios
            var pet_position = (ubicaciones_refugios)value;

            //Calc distance between user and pet (0 == KM, 1 == Miles)
            return $"{Math.Round(Location.CalculateDistance(current_position.Latitude, current_position.Longitude, pet_position.lalitud, pet_position.longitud, 0), 2)} Km";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
