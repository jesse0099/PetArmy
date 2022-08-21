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
using PetArmy.Models.GrapQLRequests;
using PetArmy.Models.GrapQLRequests.UpdatePetRequestModels;

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

        public List<Imagen_Mascota> BDImages { get; set; }

        //Tupla <Id_mascota, Id_imagen>
        public List<Tuple<Tuple<int, int>, string>> UpdatedImages { get; set; }

        //Tupla <Id_mascota, Id_imagen>
        public List<Tuple<int, int>> DeletedImages { get; set; }

        public List<Tuple<Tuple<int, int>, string>> AddedImages { get; set; }




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
            UpdatedImages = new();
            AddedImages = new();
            DeletedImages = new();
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
        public ICommand DeleteImage
        {
            get
            {
                return new Command((e) =>
                {
                    DeleteImagesExecute(e as Imagen_Mascota);
                });
            }
        }
        #endregion

        #region Functions
        public async void updatePet()
        {
            try
            {
                await MascotaService.updateMascota(currPet, AddedImages.ConvertAll<ImagenMascotaInsertRequest>(x =>
                {
                    return new()
                    {
                        image = x.Item2,
                        idDefault = false,
                        id_mascota = x.Item1.Item1,
                        id_imagen = x.Item1.Item2
                    };
                }), UpdatedImages.ConvertAll<UpdatedImage>((y) => new()
                {
                    where = new()
                    {
                        _and = new List<And>()
                        {
                            new (){
                                id_imagen = new(){
                                    _eq =  y.Item1.Item2
                                },
                                id_mascota = new()
                                {
                                    _eq = y.Item1.Item1
                                }
                            }
                        }
                    },
                    _set = new()
                    {
                        image = y.Item2
                    }
                }), DeletedImages.ConvertAll((z) => { return z.Item2; }));
                AddedImages.Clear();
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
                    if (ImagenesMascota.Count != 0)
                    {
                        tmp_index = ImagenesMascota.Max((x) => x.id_imagen) + 1;
                    }

                    picked_image = await PickImage(CurrentPet.id_mascota, tmp_index);
                    ImagenesMascota.Add(picked_image.Item1);
                    var tmp = ImagenesMascota;
                    ImagenesMascota = new BindingList<Imagen_Mascota>(tmp);
                    AddedImages.Add(new Tuple<Tuple<int, int>, string>(new Tuple<int, int>(picked_image.Item1.id_mascota, picked_image.Item1.id_imagen),
                        Convert.ToBase64String(picked_image.Item2)));
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

                     //Is BD Image? 
                     if (IsBDImage(image))
                     {
                         if (UpdatedImages.FindIndex((x) => x.Item1.Item1 == CurrentPet.id_mascota && x.Item1.Item2 == image.id_imagen) == -1)
                         {
                             //Primera Modificacion
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
                     }
                     else
                     {
                         //Modificacion sobre una imagen local
                         ImagenesMascota[global_list_index] = picked_image.Item1;
                         AddedImages[AddedImages.FindIndex((x) => x.Item1.Item1 == image.id_mascota && x.Item1.Item2 == image.id_imagen)] =
                            new Tuple<Tuple<int, int>, string>(new Tuple<int, int>(picked_image.Item1.id_mascota, picked_image.Item1.id_imagen), Convert.ToBase64String(picked_image.Item2));
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

        public void DeleteImagesExecute(Imagen_Mascota imageToDelete)
        {
            try
            {

                //Check if selected image is in database
                if (IsBDImage(imageToDelete))
                {
                    //Agregada a lista de operaciones de eliminacion 
                    DeletedImages.Add(new Tuple<int, int>(imageToDelete.id_mascota, imageToDelete.id_imagen));
                }
                else
                {
                    //Eliminar de lista de operaciones de adicion
                    AddedImages.RemoveAt(AddedImages.FindIndex((x) => x.Item1.Item1 == imageToDelete.id_mascota && x.Item1.Item2 == imageToDelete.id_imagen));
                }

                //Eliminando Imagen de la lista local
                ImagenesMascota.Remove(imageToDelete);

                var tmpUiList = ImagenesMascota;

                ImagenesMascota = new BindingList<Imagen_Mascota>(tmpUiList);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private bool IsBDImage(Imagen_Mascota compare)
        {
            if (BDImages == null)
                return false;
            if (BDImages.Count() == 0)
                return false;

            return BDImages.FindIndex((x) => x.id_imagen == compare.id_imagen && x.id_mascota == compare.id_mascota) != -1;
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
                return new Tuple<Imagen_Mascota, byte[]>(new Imagen_Mascota()
                {
                    imagen = Convert.ToBase64String(bytes, 0, bytes.Length),
                    id_mascota = idMascota,
                    id_imagen = idImagen
                },
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
