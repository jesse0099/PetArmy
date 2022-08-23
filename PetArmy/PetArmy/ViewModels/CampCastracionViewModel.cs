using PetArmy.Helpers;
using PetArmy.Infraestructure;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Services;
using PetArmy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private ICommand _getAllCampCastra;

        public ICommand GetAllCampCastra
        {
            get { return _getAllCampCastra; }
            set { _getAllCampCastra = value;
                OnPropertyChanged();
            }
        }

        public ICommand Add_CampCastra { get; set; }

        public ICommand Edit_CampCastra
        {
            get {
                return new Command((e) => {
                    editCampCastra((Camp_Castracion)e);
                });
            }
        }

        public void initCommands()
        {
            GetAllCampCastra = new Command(getCampCastra);
            Add_CampCastra = new Command(addCampCastra);
        }

        public async void getCampCastra()
        {
            IsBusy = true;
            try
            {
                if (!string.IsNullOrEmpty(Settings.UID))
                    CampCastraList = new BindingList<Camp_Castracion>(await GraphQLService.getAllCampCastra());
            }
            catch (Exception e)
            {
                Console.WriteLine(nameof(CampCastracionViewModel), e.Message);
            }
            IsBusy = false;
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

        public async void editCampCastra(Camp_Castracion param)
        {
            try
            {
                IsBusy = true;
                App.Current.Resources.TryGetValue("Locator", out object locator);
                ((InstanceLocator)locator).Main.EditCampCastra.CurCampCastra = param;
                IsBusy = false;
                await App.Current.MainPage.Navigation.PushAsync(new EditCampCastra());
            }
            catch (Exception)
            {

                throw;
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