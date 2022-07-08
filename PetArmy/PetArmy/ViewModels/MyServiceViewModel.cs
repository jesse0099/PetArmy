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

        public async void initClass()
        {
            try
            {   /* Check if user ID is set in general settings*/
                if (!String.IsNullOrEmpty(Settings.UID))
                {

                    myShelters = await GraphQLService.shelters_ByUser(Settings.UID);

                    if (myShelters.Count > 0)
                    {

                        /* For each shelter get images collection */
                        for (int i=0; i <= myShelters.Count-1;i++)
                        {
                            curShelter = myShelters[i];
                            await buildList(curShelter);
                        }

                    }
                    else
                    {
                        /* No shelters behaviors */
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

        private List<CstmItemRefugio> customList;

        public List<CstmItemRefugio> CustomList
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

        public async Task buildList(Refugio shelter)
        {
            imagesCollection = await GraphQLService.getAllImages();

            /*Search for the default picture, if not set by user use default */
          

            for (int i=0; i <= imagesCollection.Count-1;i++ )
            {
                if (imagesCollection[i].isDefault)
                {
                    hasDefault = true;
                    imageData = Convert.FromBase64String(imagesCollection[i].imagen);
                }
            }

            /*Add custom item to list*/
            if (hasDefault)
            {
                /* sets user default image for shelter*/
                createAndAdd(shelter,imageData);
            }
            else
            {
                /*Sets default image for shelter*/
                imageData = Convert.FromBase64String(String64Images.shelterDefault);
                createAndAdd(shelter, imageData);
            }
        }


        public void createAndAdd(Refugio shelter, byte[] imageData)
        {
            newItem.refugio = shelter;
            newItem.Image = imageData;
            customList.Add(newItem);
        }

        #endregion

    }
}
