using PetArmy.Helpers;
using PetArmy.Models;
using PetArmy.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
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
            set
            {
                _imagenes_mascota = value;
                OnPropertyChanged();
            }
        }

        //Tupla <Id_mascota, Id_imagen>
        public List<Tuple<Tuple<int, int>, string>> UpdatedImages { get; set; }

        //Tupla <Id_mascota, Id_imagen>
        public List<Tuple<int, int>> DeletedImages { get; set; }

        public List<string> AddedImages { get; set; }




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
            UpdatedImages = new List<Tuple<Tuple<int, int>, string>>();
            AddedImages = new List<string>();
            DeletedImages = new List<Tuple<int, int>>();
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
        public ICommand UpdateImage
        {
            get
            {
                return new Command((e) =>
                {
                    UpdateImageExecute(e as Imagen_Mascota);
                });
            }
        }
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
                    int tmp_index = 1;
                    Tuple<Imagen_Mascota, byte[]> picked_image = null;
                    if (ImagenesMascota.Count != 0){
                        tmp_index = ImagenesMascota.Max((x) => x.id_imagen) + 1;
                    }

                    picked_image = await PickImage(CurrentPet.id_mascota, tmp_index);
                    ImagenesMascota.Add(picked_image.Item1);
                    var tmp = ImagenesMascota;
                    ImagenesMascota = new BindingList<Imagen_Mascota>(tmp);
                    AddedImages.Add(Convert.ToBase64String(picked_image.Item2));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Tupla <Id_mascota, Id_imagen>
        public async void UpdateImageExecute(Imagen_Mascota image)
        {
            try
            {
                await Task.Run(async () =>
                 {

                     var picked_image = await PickImage(CurrentPet.id_mascota, image.id_imagen);
                     var global_list_index = ImagenesMascota.IndexOf(picked_image.Item1);

                     if (UpdatedImages.FindIndex((x) => x.Item1.Item1 == CurrentPet.id_mascota && x.Item1.Item2 == image.id_imagen) == -1)
                     {
                         var updated = new Tuple<Tuple<int, int>, string>(new Tuple<int, int>(image.id_mascota, image.id_imagen), 
                             Convert.ToBase64String(picked_image.Item2));
                         UpdatedImages.Add(updated);
                         
                         ImagenesMascota[global_list_index] = picked_image.Item1;
                     }
                     else
                     {
                         //Modificaciones anteriores
                         var editing_index = UpdatedImages.FindIndex((x) => x.Item1.Item1 == CurrentPet.id_mascota && x.Item1.Item2 == image.id_imagen);

                         UpdatedImages[editing_index] = new Tuple<Tuple<int, int>, string>(new Tuple<int, int>(image.id_mascota, image.id_imagen), 
                             Convert.ToBase64String(picked_image.Item2));
                         
                         ImagenesMascota[global_list_index] = picked_image.Item1;
                     }
                     var tmp = ImagenesMascota;
                     ImagenesMascota = new BindingList<Imagen_Mascota>(tmp);

                 });

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<Tuple<Imagen_Mascota, byte[]>> PickImage(int idMascota, int idImagen)
        {
            try
            {
                var mediaOptions = new PickMediaOptions() { PhotoSize = PhotoSize.Medium };
                var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                var imgSource = ImageSource.FromStream(() => selectedImage.GetStream());
                Stream stream = Commons.GetImageSourceStream(imgSource);
                var bytes = Commons.StreamToByteArray(stream);
                return new Tuple<Imagen_Mascota, byte[]>(new Imagen_Mascota() { imagen = Convert.ToBase64String(bytes, 0, bytes.Length), 
                    id_mascota = idMascota, 
                    id_imagen = idImagen }, 
                    bytes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion


    }
}
