using PetArmy.Infraestructure;
using PetArmy.Interfaces;
using PetArmy.Views;
using System;
using Xamarin.Forms;

namespace PetArmy
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        IFirebaseAuth _i_auth;


        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            _i_auth = DependencyService.Get<IFirebaseAuth>();
            if (_i_auth.SignOut())
            {
                object output;
                Xamarin.Forms.Application.Current.Resources.TryGetValue("Locator", out output);
                ((InstanceLocator)output).Main.Login.RefreshUiState();
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
                await DisplayAlert("Error", "Error", null);
        }
    }
}
