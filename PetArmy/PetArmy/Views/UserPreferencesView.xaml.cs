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
    public partial class UserPreferencesView : ContentPage
    {
        public UserPreferencesView()
        {
            InitializeComponent();
        }

        private void tagIsActive_StateChanged(object sender, Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs e)
        {
            App.Current.Resources.TryGetValue("Locator", out object locator);
            Task.Run(async () => { await ((InstanceLocator)locator).Main.UserProfile.updatePreferences(); });
        }
    }
}