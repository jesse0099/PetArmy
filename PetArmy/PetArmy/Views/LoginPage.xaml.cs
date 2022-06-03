using PetArmy.Interfaces;
using PetArmy.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using PetArmy.Models;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IFirebaseAut _i_auth;
        IGoogleAuth _g_auth;
        IFacebookAuth _f_auth;
        public LoginPage()
        {
            InitializeComponent();
            _i_auth = DependencyService.Get<IFirebaseAut>();
            _g_auth = DependencyService.Get<IGoogleAuth>();
            _f_auth = DependencyService.Get<IFacebookAuth>();
        }

        async void OnLoginClicked(object sender, EventArgs e) {
            string Token = await _i_auth.LoginWithEmailAndPassword(email.Text, password.Text);
            if (Token != "")
            {
                await Shell.Current.GoToAsync("//AboutPage");
            }
            else
            {
                ShowError();
            }
        }
        void OnGoogleLoginClicked(object sender, EventArgs e) {
            _g_auth.Login(OnGoogleLoginComplete);
        }
        void OnFacebookLoginClicked(object sender, EventArgs e) {
            _f_auth.Login(OnFacebookLoginComplete);
        }
        
        async private void OnGoogleLoginComplete(UserProfile googleUser, string message)
        {
            if (googleUser != null)
            {
                //Logueado con google
                await Shell.Current.GoToAsync("//AboutPage");
            }
            else
            {
                await DisplayAlert("Message", message, "Ok");
            }
        }
       
        async private void OnFacebookLoginComplete(UserProfile googleUser, string message)
        {
            if (googleUser != null)
            {
                //Logueado con google
                await Shell.Current.GoToAsync("//AboutPage");
            }
            else
            {
                await DisplayAlert("Message", message, "Ok");
            }
        }
        async private void ShowError()
        {
            await DisplayAlert("Authentication Failed", "E-mail or password are incorrect. Try again!", "OK");
        }
    }
}