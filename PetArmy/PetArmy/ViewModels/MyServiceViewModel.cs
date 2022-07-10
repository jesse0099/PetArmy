using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PetArmy.Services;
using Xamarin.Forms;
using PetArmy.Models;
using PetArmy.Views;
using PetArmy.Interfaces;
using PetArmy.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PetArmy.ViewModels
{
    public class MyServiceViewModel : BaseViewModel
    {

        #region Singleton
        public static MyServiceViewModel instance = null;

        public static MyServiceViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MyServiceViewModel();
            }
            return instance;
        }

        public void DeleteInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        public MyServiceViewModel()
        {
            initCommands();
            initClass();
            
        }

        public void initClass()
        {
           
        }

        public void initCommands()
        {
            NewShelter = new Command(newShelter);
            PickImage = new Command(pickImage);
           
        }

        #endregion

        #region Variables

        IFirebaseAuth _i_auth;

        private BindingList<CstmItemRefugio> customList;

        public BindingList<CstmItemRefugio> CustomList
        {
            get { return customList; }
            set { customList = value; OnPropertyChanged(); }
        }

        private List<Imagen_refugio> imagesCollection;

        public List<Imagen_refugio> ImagesCollection
        {
            get { return imagesCollection; }
            set { imagesCollection = value; OnPropertyChanged(); }
        }

        private List<Refugio> myShelters;

        public List<Refugio> MyShelters
        {
            get { return myShelters; }
            set { myShelters = value; OnPropertyChanged(); }
        }

        private CstmItemRefugio newItem;

        public CstmItemRefugio NewItem
        {
            get { return newItem; }
            set { newItem = value; OnPropertyChanged(); }
        }

        private bool hasDefault;

        public bool HasDefault
        {
            get { return hasDefault; }
            set { hasDefault = value; OnPropertyChanged(); }
        }

        private byte[] imageData;

        public byte[] ImageData
        {
            get { return imageData; }
            set { imageData = value; OnPropertyChanged(); }
        }

        private Refugio curShelter;

        public Refugio CurShelter
        {
            get { return curShelter; }
            set { curShelter = value; OnPropertyChanged(); }
        }

        private ImageSource imgSource;

        public ImageSource ImgSource
        {
            get { return imgSource; }
            set { imgSource = value; OnPropertyChanged(); }
        }

        public BindingList<string> dummy = new BindingList<string> { "jueputa", "mierda", "perra" };


        #endregion

        #region Commands and Funtions

        public ICommand NewShelter { get; set; }
        public ICommand PickImage { get; set; }
       
    
        public async void newShelter()
        {
            try
            {
                /*await GraphQLService.GetMockString().ConfigureAwait(false);
                /* await GraphQLService.Insert_MockString("insertados").ConfigureAwait(false);*/
                /* await GraphQLService.Get_MockString_ById("s1").ConfigureAwait(false);*/

                await Application.Current.MainPage.Navigation.PushAsync(new NewShelterView());

            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("xxxxd", e.ToString(), "Ok");
            }

        }

        public async Task getMyShelters(string uid)
        {
            try
            {
               
               await GraphQLService.getAllImages();
            }
            catch (Exception)
            {

                throw;
            }
    
        }

    
        public async Task getData()
        {
            IsBusy = true;
            List<CstmItemRefugio> temp = new List<CstmItemRefugio>();

            if (!String.IsNullOrEmpty(Settings.UID))
            {
                /* Get all user's shelters */
                List<Refugio> myShelters = await GraphQLService.shelters_ByUser(Settings.UID);

                if (myShelters.Count > 0)
                {
                    /* For each shelter get images collection */
                    foreach (Refugio shelter in myShelters)
                    {
                        imagesCollection = await GraphQLService.getImages_ByShelter(shelter.id_refugio);

                        /*Search for the default picture, if not set by user use default */
                        bool hasDefault = false;
                        byte[] imageData = null;

                        foreach (Imagen_refugio image in imagesCollection)
                        {
                            if (image.isDefault)
                            {
                                hasDefault = true;
                                imageData = Convert.FromBase64String(image.imagen);
                            }
                        }

                        /*Add custom item to list*/
                        CstmItemRefugio newItem = new CstmItemRefugio();
                        if (hasDefault)
                        {
                            /* sets user default image for shelter*/
                            newItem.refugio = shelter;
                            newItem.Image = imageData;
                            temp.Add(newItem);
                           

                        }
                        else
                        {
                            /*Sets default image for shelter*/
                            imageData = Convert.FromBase64String(String64Images.shelterDefault);
                            newItem.refugio = shelter;
                            newItem.Image = imageData;
                            temp.Add(newItem);
                           

                        }

                    }
                    
                }
                else
                {
                    /* No shelters behaviors */
                }

            }

            CustomList = new BindingList<CstmItemRefugio>(temp);
            IsBusy = false;
        }


        public async void pickImage()
        {
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
                    Imagen_refugio imagen_Refugio = new Imagen_refugio(await GraphQLService.countAllImages()+1,3, Convert.ToBase64String(bytes, 0, bytes.Length), true);
                    await GraphQLService.addImage(imagen_Refugio);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

    }
}
