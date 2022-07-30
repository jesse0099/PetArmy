using PetArmy.Helpers;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Services;
using PetArmy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class CampCastracionViewModel : BaseViewModel
    {

        #region Singleton
        public static CampCastracionViewModel _instance = null;
        public static CampCastracionViewModel GetInstance()
        {
            if (_instance == null)
            {

                _instance = new CampCastracionViewModel();

            }
            return _instance;
        }

        public CampCastracionViewModel()
        {
            CampCastraList = new BindingList<Camp_Castracion>();
            initCommands();
            initClass();
        }

        public void initClass()
        {
            
        }
        #endregion

        #region Variables
        IFirebaseAuth _i_auth;

        private Camp_Castracion newItemCampCastra;

        public Camp_Castracion NewItemCampCastra
        {
            get { return newItemCampCastra; }
            set { newItemCampCastra = value; OnPropertyChanged(); }
        }

        private BindingList<Camp_Castracion> campCastraList;

        public BindingList<Camp_Castracion> CampCastraList
        {
            get { return campCastraList; }
            set { campCastraList = value; OnPropertyChanged(); }
        }
        #endregion
        public ICommand getAllCampCastra { get; set; }

        public ICommand Add_CampCastra { get; set; }

        public void initCommands()
        {
            getAllCampCastra = new Command(getCampCastra);
            Add_CampCastra = new Command(addCampCastra);
        }

        public async void getCampCastra()
        {
            try
            {
                await GraphQLService.getAllCampCastra();
            }
            catch
            {
            }
        }

        public async void addCampCastra()
        {
            
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new AddCampCastraView());
            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Couldn't open 'Add Campaña Castración'", e.ToString(), "Ok");
            }
            
        }

        public async Task getData()
        {
            IsBusy = true;
            List<Camp_Castracion> tempCamp = new List<Camp_Castracion>();

            if (!String.IsNullOrEmpty(Settings.UID))
            {
                List<Camp_Castracion> myCampaings = await GraphQLService.getAllCampCastra();

                if (myCampaings.Count > 0)
                {
                    foreach (Camp_Castracion campaing in myCampaings)
                    {
                        /*Add campaing item to list*/
                        Camp_Castracion newItem = new Camp_Castracion(campaing.id_campana, campaing.nombre_camp, campaing.descripcion, campaing.direccion, campaing.tel_contacto);
                        tempCamp.Add(newItem);
                    }
                }

                CampCastraList = new BindingList<Camp_Castracion>(tempCamp);
                IsBusy = false;
            }
        }
    }
}