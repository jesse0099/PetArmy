using PetArmy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using PetArmy.Helpers;
using PetArmy.Services;
using PetArmy.Interfaces;
using System.Collections.ObjectModel;
using Plugin.Geolocator;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;
using System.Diagnostics;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Resx;

namespace PetArmy.ViewModels
{
    public class NewCasaCunaViewModel : BaseViewModel
    {
        #region Singleton
        public static NewCasaCunaViewModel instance = null;

        public static NewCasaCunaViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new NewCasaCunaViewModel();
            }
            return instance;
        }

        public static void DeleteInstnace()
        {
            if (instance != null)
            {
                instance = null;
            }
        }


        public NewCasaCunaViewModel()
        {
            initClass();
            initCommads();
        }

        public void initClass() { }
        public void initCommads()
        {

            AddCasaCuna = new Command(addNewCasaCuna);
        }

        #endregion

        #region Variables
        private string uid;

        public string UID
        {
            get { return uid; }
            set { uid = value; OnPropertyChanged(); }
        }


        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged(); }
        }


        private string correo;

        public string Correo
        {
            get { return correo; }
            set { correo = value; OnPropertyChanged(); }
        }

        private string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; OnPropertyChanged(); }
        }

        private string direccion;

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; OnPropertyChanged(); }
        }

        #region Map

        #region Latitude
        private double lat = 0.0;
        public double Latitude
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
                OnPropertyChanged("Identifier");
            }
        }
        #endregion

        #region Longitude
        private double longt = 0.0;
        public double Longitude
        {
            get
            {
                return longt;
            }
            set
            {
                longt = value;
                OnPropertyChanged("Identifier");
            }
        }


        #endregion

        #region Altitude

        private double alt = 0.0;
        public double Altitude
        {
            get
            {
                return alt;
            }
            set
            {
                alt = value;
                OnPropertyChanged("Identifier");
            }
        }

        #endregion

        #region Map Builder
        private ObservableCollection<UsersLocation> _lstLocations = new ObservableCollection<UsersLocation>();

        public ObservableCollection<UsersLocation> lstLocations
        {
            get { return _lstLocations; }

            set
            {
                _lstLocations = value;
                OnPropertyChanged("lstLocations");
            }
        }

        private Position myPosition;

        public Position MyPosition
        {
            get { return myPosition; }
            set { myPosition = value; OnPropertyChanged(); }
        }


        #endregion

        public double newLat = 0.0;
        public double newLong = 0.0;

        #endregion


        public IList<string> Cantones
        {

            get { return new List<string> { "San José", "Cartago", "Heredia", "Alajuela", "Puntarenas", "Limón", "Guanacaste" }; }

        }

        private string canton;

        public string Canton
        {
            get { return canton; }
            set { canton = value; OnPropertyChanged(); }
        }

        #endregion

        #region Funtions and Commands


        public ICommand AddCasaCuna { get; set; }



        public bool checkEmptyValues()
        {
            bool anyEmpty = false;
            if (String.IsNullOrEmpty(Nombre) || String.IsNullOrEmpty(Correo) || String.IsNullOrEmpty(Direccion) || String.IsNullOrEmpty(Telefono))
            {
                anyEmpty = true;
            }
            return anyEmpty;
        }


        public async void addNewCasaCuna()
        {

            if (!checkEmptyValues())
            {

                if (!String.IsNullOrEmpty(Settings.UID))
                {
                    try
                    {
                        Perfil_adoptante curAdoptante = null;
                        curAdoptante = await GraphQLService.getAdoptanteByID(Settings.UID);

                        if (curAdoptante == null)
                        {
                            curAdoptante = new Perfil_adoptante();
                            curAdoptante.uid = Settings.UID;
                            curAdoptante.nombre = Nombre;
                            curAdoptante.correo = Correo;
                            curAdoptante.direccion = Direccion;
                            curAdoptante.telefono = Telefono;
                            curAdoptante.casa_cuna = true;

                            await GraphQLService.addAdoptante(curAdoptante);

                            ubicaciones_casasCuna newCasaCuna = new ubicaciones_casasCuna();
                            newCasaCuna.id_ubicacion = await GraphQLService.countCasasCuna() + 1;
                            newCasaCuna.id_user = Settings.UID;
                            newCasaCuna.canton = Canton;
                            newCasaCuna.lalitud = Latitude;
                            newCasaCuna.longitud = Longitude;

                            await GraphQLService.addCasaCunaLocation(newCasaCuna);

                        }
                        else
                        {

                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            }
            else
            {
                ErrorTitle = AppResources.errorEmptyValues;
                ErrorMessage = AppResources.errorEmptyValues;
                OpenPopUp = true;
            }
        }

        public static async Task<Position> GetCurrentPosition()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);

            return position;
        }
        public async Task setCurrentLocation()
        {
            /* Gets current location*/
            Position position = await GetCurrentPosition();
            /*Creates Item for the map*/
            UsersLocation curLocation = new UsersLocation();
            /*Sets pins values*/
            curLocation.Title = "Current Location";
            curLocation.Longitude = position.Longitude;
            curLocation.Latitude = position.Latitude;
            curLocation.Description = "This is your current location";
            /*Sets Variables*/
            Latitude = position.Latitude;
            Longitude = position.Longitude;
            /* Adds pin */
            lstLocations.Add(curLocation);
        }
        #endregion

    }
}






