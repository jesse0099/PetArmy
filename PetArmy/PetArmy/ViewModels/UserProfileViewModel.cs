using PetArmy.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using Plugin.Geolocator;
using PetArmy.Services;
using System.ComponentModel;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System.IO;
using PetArmy.Helpers;
using System.Windows.Input;
using PetArmy.Interfaces;
using PetArmy.Views;

namespace PetArmy.ViewModels
{
    public class UserProfileViewModel: BaseViewModel
    {
        #region Singleton
        public static UserProfileViewModel instance = null;

        public static UserProfileViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new UserProfileViewModel();
            }
            return instance;
        }

        public static void DisposeInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        public UserProfileViewModel()
        {
            initClass();
            initCommands();
        }

        public void initClass()
        {
            _i_auth = DependencyService.Get<IFirebaseAuth>();
         
        }

        public async void initCommands()
        {
            await setUserInfo();
            OpenUserInfo = new Command(openUserInfo);
            PickImage = new Command(pickImage);
            EditInfo = new Command(editInfo);
        }

        #endregion

        #region Variables 

        IFirebaseAuth _i_auth;

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private User_Info curUser;

        public User_Info CurUser
        {
            get { return curUser; }
            set { curUser = value; OnPropertyChanged(); }
        }

        private string _name;

        public string Name 
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _surname;

        public string Surname
        {
            get { return _surname; }
            set { _surname = value; OnPropertyChanged(); }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        private int _age;

        public int Age
        {
            get { return _age; }
            set { _age = value; OnPropertyChanged(); }
        }

        private string _canton;

        public string Canton
        {
            get { return _canton; }
            set { _canton = value; OnPropertyChanged(); }
        }

        private string _ubication;

        public string Ubication
        {
            get { return _ubication; }
            set { _ubication = value; OnPropertyChanged(); }
        }

        private bool _hasInfo;

        public bool HasInfo
        {
            get { return _hasInfo; }
            set { _hasInfo = value; OnPropertyChanged(); }
        }

        private byte[] _imageData;

        public byte[] ImageData
        {
            get { return _imageData; }
            set { _imageData = value; OnPropertyChanged(); }
        }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _sAge;

        public string SAge
        {
            get { return _sAge; }
            set { _sAge = value; OnPropertyChanged(); }
        }

        public IList<string> Cantones
        {

            get { return new List<string> { "San José", "Cartago", "Heredia", "Alajuela", "Puntarenas", "Limón", "Guanacaste" }; }

        }

        private ImageSource imgSource;

        public ImageSource ImgSource
        {
            get { return imgSource; }
            set { imgSource = value; OnPropertyChanged(); }
        }

        private bool imageIsSelected;

        public bool ImageIsSelected
        {
            get { return imageIsSelected; }
            set { imageIsSelected = value; OnPropertyChanged(); }
        }

        private string imageString="";

        public string ImageString
        {
            get { return imageString; }
            set { imageString = value; OnPropertyChanged(); }
        }


        private bool dataIsFound;

        public bool DataIsFound
        {
            get { return dataIsFound; }
            set { dataIsFound = value; OnPropertyChanged(); }
        }


        #endregion

        #region Commands and Functions

        public ICommand GetUserInfo { get; set; }
        public ICommand OpenUserInfo { get; set; }
        public ICommand PickImage { get; set; }
        public ICommand EditInfo { get; set; }


        public async Task setUserInfo()
        {
            List<User_Info> usersInfo = new List<User_Info>();
            usersInfo = await GraphQLService.getUserInfo_ByUID(Settings.UID);

            if (usersInfo.Count > 0)
            {
                /* Sets user info */
                foreach (User_Info item in usersInfo)
                {
                    CurUser = item;
                }

                ImageData = Convert.FromBase64String(CurUser.profilePicture);
                FullName = CurUser.name+" "+CurUser.surname;
                Name = CurUser.name;
                Surname = CurUser.surname;
                Username = CurUser.username;
                SAge = CurUser.age.ToString();
                Ubication = "Costa Rica, "+CurUser.canton;
                DataIsFound = true;

            }
            else
            {
                /* Sets default info */
                ImageData = Convert.FromBase64String(String64Images.profileDefault);
                FullName = "Sample Name";
                Username = "Sample Username";
                SAge = "Your Age";
                Ubication = "Sample Ubication";
                DataIsFound = false;

            }
           
        }

        public async void openUserInfo()
        {
            try
            {
                if (!DataIsFound)
                {
                    FullName = "";
                    Username = "";
                    SAge = "";
                    Ubication = "";
                }
                await Application.Current.MainPage.Navigation.PushAsync(new EditUserInfoView());
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async void pickImage()
        {
            Imagen_refugio imagen_Refugio = new Imagen_refugio();
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
                    ImageString = Convert.ToBase64String(bytes, 0, bytes.Length);
                    ImageData = bytes;
                    ImageIsSelected = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void editInfo()
        {
            User_Info newInfo = new User_Info();
            newInfo.idUser = Settings.UID;
            newInfo.name = Name;
            newInfo.surname = Surname;
            newInfo.canton = Canton;
            newInfo.age = Int32.Parse(SAge); ;
            newInfo.username = Username;
            if (imageIsSelected)
            {
                newInfo.profilePicture = ImageString;
            }
            else
            {
                newInfo.profilePicture = Convert.ToBase64String(ImageData, 0, ImageData.Length);
            }

            await GraphQLService.createOrUpdate_UserInfo(DataIsFound,newInfo);

            FullName = newInfo.name + " " + newInfo.surname;
            Ubication = "Costa Rica, " + newInfo.canton;
            SAge = newInfo.age.ToString();
            Username = newInfo.username;
            DataIsFound = true;
            await Shell.Current.GoToAsync("//UserProfileTabbedView");
        }


        #endregion

    }
}
