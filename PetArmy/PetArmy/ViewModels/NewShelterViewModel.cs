﻿using PetArmy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using PetArmy.Helpers;
using PetArmy.Services;
using PetArmy.Interfaces;

namespace PetArmy.ViewModels
{
    public class NewShelterViewModel : BaseViewModel
    {

        #region Singleton

        public static NewShelterViewModel Instance = null;


        public static NewShelterViewModel GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NewShelterViewModel();
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

        public NewShelterViewModel()
        {
            initCommands();
            initClass();
        }

        public void initCommands()
        {
            CreateShelter = new Command(createShelter);
        }

        public void initClass()
        {

        }


        #endregion


        #region Varaiables

        private int quantSpace = 0;

        public int QuantSpace
        {
            get { return quantSpace = 1; }
            set { quantSpace = value; OnPropertyChanged(); }
        }

        private string shelterName;

        public string ShelterName
        {
            get { return shelterName; }
            set { shelterName = value; OnPropertyChanged(); }
        }

        private string shelterDir;

        public string ShelterDir
        {
            get { return shelterDir; }
            set { shelterDir = value; OnPropertyChanged(); }
        }

        private string shelterNumber;

        public string ShelterNumber
        {
            get { return shelterNumber; }
            set { shelterNumber = value; OnPropertyChanged(); }
        }

        private string shelterEmail;

        public string ShelterEmail
        {
            get { return shelterEmail; }
            set { shelterEmail = value; OnPropertyChanged(); }
        }

        #endregion


        #region Commands and Functions


        public ICommand CreateShelter { get; set; }

        public async void createShelter()
        {
            try
            {
                IFirebaseAuth _i_auth = DependencyService.Get<IFirebaseAuth>(); ;
                var registered_user = _i_auth.GetSignedUserProfile();
                Settings.UID = registered_user.Uid;
                Usuario curUser = new Usuario(Settings.UID,2);

                if (!checkForEmpyValues())
                {
                    Refugio newShelter = new Refugio();
                    newShelter.administrador = registered_user.Uid;
                    newShelter.id_refugio = 1;
                    newShelter.nombre = shelterName;
                    newShelter.correo = shelterEmail;
                    newShelter.telefono = shelterNumber;
                    newShelter.capacidad = quantSpace;
                    newShelter.direccion = shelterDir;
                    newShelter.activo = false;

                    bool chk = await GraphQLService.createShelter(newShelter,curUser).ConfigureAwait(false); 
                }
                else
                {

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool checkForEmpyValues()
        {
            bool result = false;

            if ( String.IsNullOrEmpty(this.shelterName) || String.IsNullOrEmpty(this.shelterDir) || String.IsNullOrEmpty(this.shelterEmail) || String.IsNullOrEmpty(this.shelterNumber))
            {
                result = true;
            }

            return result;
        }

        #endregion

    }
}
