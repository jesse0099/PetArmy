using PetArmy.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewShelterView : ContentPage
    {
        public NewShelterView()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            Task.Run(async () => { await ((InstanceLocator)locator).Main.NewShelter.setCurrentLocation(); });
        }
    }
}