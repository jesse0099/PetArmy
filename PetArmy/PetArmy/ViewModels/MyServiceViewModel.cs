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
                    /* Get all user's shelters */
                    List<Refugio> myShelters = await GraphQLService.shelters_ByUser(Settings.UID);

                    if (myShelters.Count > 0)
                    {
                        List<Imagen_refugio> imagesCollection = new List<Imagen_refugio>();
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
                                customList.Add(newItem);
                            }
                            else
                            {
                                /*Sets default image for shelter*/
                                imageData = Convert.FromBase64String(String64Images.shelterDefault);
                                newItem.refugio = shelter;
                                newItem.Image = imageData;
                                customList.Add(newItem);
                            }

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

        #endregion

    }
}
