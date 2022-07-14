using PetArmy.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetArmy.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyServicesView : ContentPage
    {
        public MyServicesView()
        {
            InitializeComponent();
         
        }

        protected override void OnAppearing()
        {
           base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            Task.Run(async () => { await ((InstanceLocator)locator).Main.MyService.getData(); });
            Task.Run(async () => { await ((InstanceLocator)locator).Main.NewShelter.setCurrentLocation(); });
            Task.Run(async () => { await ((InstanceLocator)locator).Main.NewCasaCuna.setCurrentLocation(); });

        }
    }
}