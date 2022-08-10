﻿using PetArmy.Models;
using PetArmy.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class EditCampCastraViewModel : BaseViewModel
    {
        #region Singleton
        public static EditCampCastraViewModel _instance = null;
        public static EditCampCastraViewModel GetInstance()
        {
            if (_instance == null)
            {

                _instance = new EditCampCastraViewModel();

            }
            return _instance;
        }

        public static void DisposeInstance()
        {
            if (_instance != null)
            {
                _instance = null;
            }
        }

        public EditCampCastraViewModel()
        {
            initCommands();
            initClass();
        }

        public void initClass()
        {

        }

        public void initCommands()
        {
            UpdateCampCastra = new Command(updateCampCastra);
            DeleteCampCastra = new Command(deleteCampCastra);
        }
        #endregion

        #region Varaiables

        private Camp_Castracion curCampCastra;

        public Camp_Castracion CurCampCastra
        {
            get { return curCampCastra; }
            set { curCampCastra = value; 
                OnPropertyChanged(); 
            }
        }
        #endregion

        #region Commands
        public ICommand UpdateCampCastra { get; set; }
        public ICommand DeleteCampCastra { get; set; }



        public async void updateCampCastra()
        {
            await GraphQLService.updateCampCastra(CurCampCastra);
            await Shell.Current.GoToAsync("//CampCastraView");

        }

        public async void deleteCampCastra()
        {
            await GraphQLService.deleteCampCastra(CurCampCastra.id_campana);
            await Shell.Current.GoToAsync("//CampCastraView");
        }
        #endregion
    }
}
