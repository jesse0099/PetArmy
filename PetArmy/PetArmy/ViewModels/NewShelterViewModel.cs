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
using PetArmy.Views;
using PetArmy.Infraestructure;

namespace PetArmy.ViewModels
{
    public class NewShelterViewModel : BaseViewModel
    {

        #region Singleton

        public static NewShelterViewModel Instance = null;


        public static NewShelterViewModel GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NewShelterViewModel();
            }

            return Instance;
        }

        public static void DisposeInstance()
        {
            if (Instance != null)
            {
                Instance = null;
            }
        }

        public NewShelterViewModel()
        {
            initCommands();
            initClass();
        }

        public void initCommands()
        {
            CreateShelter = new Command(createShelter);
            PickImage = new Command(pickImage);
        }

        public void initClass()
        {
            
        }

        #endregion

        #region Varaiables

        private Refugio curShelter;

        public Refugio CurShelter
        {
            get { return curShelter; }
            set { curShelter = value; OnPropertyChanged(); }
        }

        private bool isEditing = false;

        public bool IsEditing
        {
            get { return isEditing; }
            set { isEditing = value; OnPropertyChanged(); }
        }

        private bool imageSelected;

        public bool ImageSelected
        {
            get { return imageSelected; }
            set { imageSelected = value; OnPropertyChanged(); }
        }

        private string quantSpace = "1";

        public string QuantSpace
        {
            get { return quantSpace; }
            set { quantSpace = value; OnPropertyChanged(); }
        }

        private string shelterName;

        public string ShelterName
        {
            get { return shelterName; }
            set { shelterName = value; OnPropertyChanged(); }
        }

        private string shelterDir;

        public string ShelterDir
        {
            get { return shelterDir; }
            set { shelterDir = value; OnPropertyChanged(); }
        }

        private string shelterNumber;

        public string ShelterNumber
        {
            get { return shelterNumber; }
            set { shelterNumber = value; OnPropertyChanged(); }
        }

        private string shelterEmail;

        public string ShelterEmail
        {
            get { return shelterEmail; }
            set { shelterEmail = value; OnPropertyChanged(); }
        }

        #region Map


        #region Latitude
        private double lat = 0.0;
        public double Latitude
        {
            get
            {
                return this.lat;
            }
            set
            {
                this.lat = value;
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
                return this.longt;
            }
            set
            {
                this.longt = value;
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
                return this.alt;
            }
            set
            {
                this.alt = value;
                OnPropertyChanged("Identifier");
            }
        }

        #endregion

        #region Mapa
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
            set { myPosition = value; OnPropertyChanged();}
        }


        #endregion


        public double newLat = 0.0;
        public double newLong = 0.0;


        private ImageSource imgSource;

        public ImageSource ImgSource
        {
            get { return imgSource; }
            set { imgSource = value; OnPropertyChanged(); }
        }

        private Imagen_refugio selectedImgae;

        public Imagen_refugio SelectedImage
        {
            get { return selectedImgae; }
            set { selectedImgae = value; OnPropertyChanged(); }
        }

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

        #endregion

        #region Commands and Functions

        public ICommand CreateShelter { get; set; }
        public ICommand PickImage { get; set; }

        public async void createShelter()
        {
            try
            {
                IFirebaseAuth _i_auth = DependencyService.Get<IFirebaseAuth>(); ;
                var registered_user = _i_auth.GetSignedUserProfile();
                Settings.UID = registered_user.Uid;
                Usuario curUser = new Usuario(Settings.UID,2);

                if (!checkForEmpyValues())
                {
                    Refugio newShelter = new Refugio();
                    newShelter.administrador = registered_user.Uid;
                    newShelter.id_refugio = await generateShelterID() ;
                    newShelter.nombre = shelterName;
                    newShelter.correo = shelterEmail;
                    newShelter.telefono = shelterNumber;
                    newShelter.capacidad = Int32.Parse(quantSpace);
                    newShelter.direccion = shelterDir;
                    newShelter.activo = false;
                    bool chk = await GraphQLService.createShelter(newShelter,curUser).ConfigureAwait(false);
                    if (imageSelected)
                    {
                        SelectedImage.id_refugio = newShelter.id_refugio;
                        await GraphQLService.addImage(SelectedImage);
                    }
                    ubicaciones_refugios newLocation = new ubicaciones_refugios();
                    newLocation.id_refugio = newShelter.id_refugio;
                    newLocation.id_ubicacion = await generateLocationID();
                    newLocation.longitud = Longitude;
                    newLocation.lalitud = Latitude;
                    newLocation.canton = Canton;
                    await GraphQLService.addShelterLocation(newLocation);
                    await Shell.Current.GoToAsync("//MyServicesView");

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

        public bool checkForEmpyValues()
        {
            bool result = false;

            if ( String.IsNullOrEmpty(this.shelterName) || String.IsNullOrEmpty(this.shelterDir) || String.IsNullOrEmpty(this.shelterEmail) || String.IsNullOrEmpty(this.shelterNumber))
            {
                result = true;
            }

            return result;
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

        public async void pickImage()
        {
            Imagen_refugio imagen_Refugio = new Imagen_refugio();
            try
            {
                imageSelected = true;
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se soporta la funcionalidad", "OK");
                }
                else
                {
                    var mediaOptions = new PickMediaOptions() { PhotoSize = PhotoSize.Medium };
                    var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                    imgSource = ImageSource.FromStream(() => selectedImage.GetStream());
                    Stream stream = Commons.GetImageSourceStream(imgSource);
                    var bytes = Commons.StreamToByteArray(stream);
                    imagen_Refugio = new Imagen_refugio(await generatePictureID(), Convert.ToBase64String(bytes, 0, bytes.Length), true);
                    SelectedImage = imagen_Refugio;
                    imageSelected = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<int> generateShelterID()
        {
            int newShelterID = 0;

            try
            {
                List<Refugio> shelters = await GraphQLService.getAllShelters();
                int lastID = shelters[shelters.Count - 1].id_refugio;
                newShelterID = lastID + 1;

            }
            catch (Exception)
            {

                throw;
            }

            return newShelterID;
        }

        public async Task<int> generatePictureID()
        {
            int newPictureID = 0;

            try
            {
                List<Imagen_refugio> images = await GraphQLService.getAllImages();
                int lastID = images[images.Count - 1].id_refugio;
                newPictureID = lastID + 1;

            }
            catch (Exception)
            {

                throw;
            }

            return newPictureID;
        }


        public async Task<int> generateLocationID()
        {
            int newLocationID = 0;

            try
            {
                List<ubicaciones_refugios> locations = await GraphQLService.getAllShelterUbications();
                int lastID = locations[locations.Count - 1].id_refugio;
                newLocationID = lastID + 1;

            }
            catch (Exception)
            {

                throw;
            }

            return newLocationID;
        }


     

        #endregion

    }
}
