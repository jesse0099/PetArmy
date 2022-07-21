using PetArmy.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using Plugin.Geolocator;
using PetArmy.Services;
using System.ComponentModel;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System.IO;
using PetArmy.Helpers;
using System.Windows.Input;

namespace PetArmy.ViewModels
{
    public class EditShelterViewModel : BaseViewModel
    {
        #region Singleton
        public static EditShelterViewModel instance = null;


        public static EditShelterViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new EditShelterViewModel();
            }
            return instance;
        }

        public static void DisposeInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }


        public EditShelterViewModel()
        {
            initClass();
            initCommands();
        }

        public void initClass()
        {

        }

        public void initCommands()
        {
            PickImage = new Command(pickImage);
            UpdateShelter = new Command(updateShelter);
            DeleteShelter = new Command(deleteShelter);
            UpdateImage = new Command<int>(SetasDefaultImg);
        }

        #endregion

        #region Varaiables


        private List<Imagen_refugio> shelterCollection;

        public List<Imagen_refugio> ShelterCollection
        {
            get { return shelterCollection; }
            set { shelterCollection = value; OnPropertyChanged(); }
        }

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


        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged(); }
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
            set { myPosition = value; OnPropertyChanged(); }
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


        private BindingList<CstmItemRefugio> customList;

        public BindingList<CstmItemRefugio> CustomList
        {
            get { return customList; }
            set { customList = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands


        public  ICommand PickImage { get; set; }  
        public ICommand UpdateShelter { get; set; }
        public ICommand DeleteShelter { get; set; } 
        public ICommand UpdateImage { get; set; }

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
                    imagen_Refugio = new Imagen_refugio(await generatePictureID(), Convert.ToBase64String(bytes, 0, bytes.Length), false);
                    imagen_Refugio.id_refugio = CurShelter.id_refugio;
                    await GraphQLService.addImage(imagen_Refugio);
                    await refreshCollection();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task readyEdit(int shelterID)
        {
            try
            {
               
                CurShelter = await GraphQLService.getShelterByID(shelterID);
                ShelterName = CurShelter.nombre;
                ShelterEmail = CurShelter.correo;
                ShelterNumber = CurShelter.telefono;
                ShelterDir = CurShelter.direccion;
                QuantSpace = CurShelter.capacidad.ToString();
                IsActive = CurShelter.activo;
                ShelterCollection = await GraphQLService.getImages_ByShelter(shelterID);
                List<CstmItemRefugio> temp = new List<CstmItemRefugio>();

                foreach (Imagen_refugio image in ShelterCollection)
                {
                    CstmItemRefugio newItem = new CstmItemRefugio();
                    newItem.Image = Convert.FromBase64String(image.imagen);
                    newItem.imgObjct = image;
                    temp.Add(newItem);
                }

                CustomList = new BindingList<CstmItemRefugio>(temp);
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task refreshCollection()
        {
            List<Imagen_refugio> newSet = await GraphQLService.getImages_ByShelter(curShelter.id_refugio);

            List<CstmItemRefugio> temp = new List<CstmItemRefugio>();

            foreach (Imagen_refugio image in newSet)
            {
                CstmItemRefugio newItem = new CstmItemRefugio();
                newItem.Image = Convert.FromBase64String(image.imagen);
                newItem.imgObjct = image;
                temp.Add(newItem);
            }

            CustomList = new BindingList<CstmItemRefugio>(temp);

        }

        public async void updateShelter()
        {
            CurShelter.nombre = ShelterName;
            CurShelter.direccion = ShelterDir;
            CurShelter.correo = ShelterEmail;
            CurShelter.activo = IsActive;
            CurShelter.capacidad = Int32.Parse(QuantSpace);
            CurShelter.telefono = ShelterNumber;

            List<ubicaciones_refugios> locations = await GraphQLService.getLocationByShelter(CurShelter.id_refugio);
            
            if (locations != null)
            {
                foreach (var location in locations)
                {
                    ubicaciones_refugios temp = location;

                    if (!String.IsNullOrEmpty(Canton))
                    {
                        temp.canton = Canton;
                        temp.lalitud = Latitude;
                        temp.longitud = Longitude;
                    }
                    else
                    {
                        temp.lalitud = Latitude;
                        temp.longitud = Longitude;
                    }

                    await GraphQLService.UpdateShelterLocation(temp);
                }
            }

            await GraphQLService.updateShelter(CurShelter);
            await Shell.Current.GoToAsync("//MyServicesView");

        }

        public async void deleteShelter()
        {
            await GraphQLService.deleteShelter(CurShelter.id_refugio);
            await Shell.Current.GoToAsync("//MyServicesView");
        }

        public async void SetasDefaultImg(int idImg)
        {
          
            foreach (var item in CustomList)
            {
                if (item.imgObjct.isDefault && item.imgObjct.id_imagen != idImg)
                {
                    /*Actualiza la imagen pasada*/
                    item.imgObjct.isDefault = false;
                    await GraphQLService.updateImage(item.imgObjct);

                }
                else if (!item.imgObjct.isDefault && item.imgObjct.id_imagen == idImg)
                {
                    /*Actualiza la imagen actual*/
                    item.imgObjct.isDefault = true;
                    await GraphQLService.updateImage(item.imgObjct);

                }
                else if(item.imgObjct.isDefault && item.imgObjct.id_imagen == idImg)
                {
                    /*Notify it's already default*/

                }
            }
        }


        #endregion

    }
}
