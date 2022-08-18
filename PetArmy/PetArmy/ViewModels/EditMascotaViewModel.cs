using PetArmy.Helpers;
using PetArmy.Models;
using PetArmy.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class EditMascotaViewModel : BaseViewModel
    {
        

        #region Singleton and Constructors
        public static EditMascotaViewModel instance = null;

        public static EditMascotaViewModel GetInstance()
        {
            return instance ??= (instance = new EditMascotaViewModel()); 
        }

        private BindingList<Imagen_Mascota> _imagenes_mascota;

        public BindingList<Imagen_Mascota> ImagenesMascota
        {
            get { return _imagenes_mascota; }
            set { _imagenes_mascota = value;
                OnPropertyChanged();    
            }
        }

        public BindingList<Imagen_Mascota> NewImages { get; set; }

        public string DefaultPetImage { get; set; }

        public static void DisposeInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        public EditMascotaViewModel()
        {
            initCommands();
            initClass();
        }

        public void initClass()
        {
            DefaultPetImage = String64Images.petDefault;
            NewImages = new BindingList<Imagen_Mascota>();
        }
        public void initCommands()
        {
            SaveMascota = new Command(updatePet);
            AddImage = new Command(AddImageExecute);
        }


        #endregion


        #region variables and Commands
        private Mascota currPet;

        public Mascota CurrentPet
        {
            get { return currPet; }
            set { currPet = value; OnPropertyChanged(); } 
        }


        public ICommand SaveMascota { get; set; }
        public ICommand AddImage { get; set; }
        #endregion

        #region Functions
        public async void updatePet()
        {
            try
            {
                await MascotaService.updateMascota(currPet);
                await Shell.Current.GoToAsync("//ListMascotasPage");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async void AddImageExecute()
        {
            Imagen_Mascota petImage = new();
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se soporta la funcionalidad", "OK");
                }
                else
                {
                    var mediaOptions = new PickMediaOptions() { PhotoSize = PhotoSize.Medium};
                    var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                    var imgSource = ImageSource.FromStream(() => selectedImage.GetStream());
                    Stream stream = Commons.GetImageSourceStream(imgSource);
                    var bytes = Commons.StreamToByteArray(stream);
                    ImagenesMascota.Add(new Imagen_Mascota() { imagen = Convert.ToBase64String(bytes, 0, bytes.Length) });
                    var tmp = ImagenesMascota;
                    ImagenesMascota = new BindingList<Imagen_Mascota>(tmp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async void pickImage()
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


    }
}
