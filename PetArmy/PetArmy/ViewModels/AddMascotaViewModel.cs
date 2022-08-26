using PetArmy.Helpers;
using PetArmy.Models;
using PetArmy.Services;
using PetArmy.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class AddMascotaViewModel : BaseViewModel
    {
        #region singleton

        private static AddMascotaViewModel Instance;
        public static AddMascotaViewModel GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AddMascotaViewModel();
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
        public AddMascotaViewModel()
        {
            initCommands();
            initClass();

        }
        public void initCommands()
        {
            AddMascota = new Command(addMascota);
        }
        public void initClass()
        {

        }

        #endregion


        #region variable
        private int id_mascota;
        private string especie;
        private string raza;
        private string nombre;
        private float peso;
        private bool castrado;
        private bool vacuando;
        private bool estado;
        private string descripcion;
        private bool discapacidad;
        private bool enfermedad;
        private bool alergias;
        private int id_refugio;

        public int Id_Mascota
        {
            get { return id_mascota; }
            set { id_mascota = value; OnPropertyChanged(); }
        }
        public string Especie
        {
            get { return especie; }
            set { especie = value; OnPropertyChanged(); }
        }
        public string Raza
        {
            get { return raza; }
            set { raza = value; OnPropertyChanged(); }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged(); }
        }
        public float Peso
        {
            get { return peso; }
            set { peso = value; OnPropertyChanged(); }
        }
        public bool Castrado
        {
            get { return castrado; }
            set { castrado = value; OnPropertyChanged(); }
        }
        public bool Vacuando
        {
            get { return vacuando; }
            set { vacuando = value; OnPropertyChanged(); }
        }
        public bool Estado
        {
            get { return estado; }
            set { estado = value; OnPropertyChanged(); }
        }
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; OnPropertyChanged(); }
        }
        public bool Discapacidad
        {
            get { return discapacidad; }
            set { discapacidad = value; OnPropertyChanged(); }
        }
        public bool Enfermedad
        {
            get { return enfermedad; }
            set { enfermedad = value; OnPropertyChanged(); }
        }
        public bool Alergias
        {
            get { return alergias; }
            set { alergias = value; OnPropertyChanged(); }
        }

        public int Id_Refugio
        {
            get { return id_refugio; }
            set { id_refugio = value; OnPropertyChanged(); }
        }

        private ImageSource imgSource;
        public ImageSource ImgSource
        {
            get { return imgSource; }
            set { imgSource = value;  OnPropertyChanged(); }
        }


        //these 2 need to be a list
        private Imagen_Mascota selectedImage;

        public Imagen_Mascota SelectedImage
        {
            get { return selectedImage; }
            set { selectedImage = value;  OnPropertyChanged(); }
        }



        #endregion


        #region commands and Functions
        public ICommand AddMascota { get; set; }

        //THIS SHOULD FILL UP A LIST
        public async void pickImages()
        {
            Imagen_Mascota petImage = new Imagen_Mascota();
            try 
            {
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
                    petImage = new Imagen_Mascota(await generatePetImageID(), Convert.ToBase64String(bytes, 0, bytes.Length), true);
                    //SelectedImage
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async void addMascota()
        {
            Mascota pet = new Mascota();
            try
            {
                //pet.id_mascota = 13;
                pet.nombre = Nombre;
                pet.id_refugio = Id_Refugio;
                pet.especie = Especie;
                pet.raza = Raza;
                pet.peso = Peso;
                pet.castrado = Castrado;
                pet.vacunado = Vacuando;
                pet.estado = Estado;
                pet.descripcion = Descripcion;
                pet.discapacidad = Discapacidad;
                pet.enfermedad = Enfermedad;
                pet.alergias = Alergias;

                await MascotaService.addMascota(pet);
                await Shell.Current.GoToAsync("//ListMascotasPage");
            }
            catch
            {
            }
        }

        private async Task<int> generatePetImageID()
        {
            int newPictureId = 0;
            try
            {
                List<Imagen_Mascota> images = await MascotaService.getAllPetImages();
                int lastID = images[images.Count - 1].id_imagen;
                newPictureId = lastID + 1;
            }
            catch (Exception)
            {
                throw;
            }
            return newPictureId;
        }

        #endregion
    }
}
