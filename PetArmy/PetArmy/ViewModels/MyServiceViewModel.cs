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

namespace PetArmy.ViewModels
{
    public class MyServiceViewModel:BaseViewModel
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
            if(instance != null)
            {
                instance = null;
            }
        }

        public MyServiceViewModel()
        {
            initCommands();
            initClass();
        }

        public async void initClass()
        {
            try
            {
                if (!String.IsNullOrEmpty(Settings.UID))
                {
                    myShelters = await GraphQLService.shelters_ByUser(Settings.UID);

                    if (myShelters.Count > 0)
                    {
                       
                        foreach (Refugio shelter in myShelters)
                        {

                            int idS = shelter.id_refugio;

                          
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        
        }

        public void initCommands()
        {
            NewShelter = new Command(newShelter);
        
        }

        #endregion

        #region Variables

        IFirebaseAuth _i_auth;

        private string _test = "Añadir lista de servicios";

        public string Test
        {
            get { return _test; }

            set
            {
                _test = value;
                OnPropertyChanged("Test");
            }
        }

        private List<Refugio> myShelters;
        public List<Refugio> MyShelters
        {
            get { return myShelters; }
            set { myShelters = value; OnPropertyChanged(); }
        }

        private Usuario curUser;

        public Usuario CurUser
        {
            get { return curUser; }
            set { curUser = value; OnPropertyChanged(); }
        }

        private MediaFileModel mediaFile = new MediaFileModel();

        public MediaFileModel MediaFile
        {
            get
            {
                return mediaFile;
            }
            set
            {
                mediaFile = value;
                OnPropertyChanged();
            }
        }


        private ImageSource imgPath = "userAlt.png";
        public ImageSource ImgPath
        {
            get
            {
                return imgPath;
            }
            set
            {
                imgPath = value;
                OnPropertyChanged();
            }
        }

        private List<CstmItemRefugio> cstListRefugio;

        public List<CstmItemRefugio> CstListRefugio
        {
            get { return cstListRefugio; }
            set { cstListRefugio = value; OnPropertyChanged(); }
        }

        private List<Imagen_refugio> myImages;

        public List<Imagen_refugio> MyImages
        {
            get { return myImages; }
            set { myImages = value; OnPropertyChanged(); }
        }



        #endregion



        #region Commands and Funtions

        public ICommand NewShelter { get; set; }

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

        
        public async void pickImage()
        {
            try
            {

                await CrossMedia.Current.Initialize();
                MediaFileModel file = new MediaFileModel();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se soporta la funcionalidad", "OK");
                }
                else
                {
                    var mediaOptions = new PickMediaOptions() { PhotoSize = PhotoSize.Medium };
                    var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                    this.ImgPath = ImageSource.FromStream(() => selectedImage.GetStream());
                    var bytes = File.ReadAllBytes(selectedImage.Path);

                    Imagen_refugio newImage = new Imagen_refugio(2,await GraphQLService.countAllImages()+1,bytes,false);

                    await GraphQLService.addImage(newImage);
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
