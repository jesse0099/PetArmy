using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PetArmy.Views
{
    class SplashScreenView : ContentPage
    {

        Image splashImage;

        public SplashScreenView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var sub = new AbsoluteLayout();
            this.splashImage = new Image
            {
                Source = "PetsArmy.png",
                WidthRequest = 250,
                HeightRequest = 250
            };
            AbsoluteLayout.SetLayoutFlags(this.splashImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(this.splashImage, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            sub.Children.Add(this.splashImage);
            OnAppThemeChange(null, new AppThemeChangedEventArgs(Application.Current.RequestedTheme));
            Application.Current.RequestedThemeChanged += OnAppThemeChange;
            this.Content = sub;
            /*Espacio*/
        }

        void OnAppThemeChange(object s, Xamarin.Forms.AppThemeChangedEventArgs a)
        {
            object output;
            switch (a.RequestedTheme)
            {
                case Xamarin.Forms.OSAppTheme.Dark:
                    {
                        Application.Current.Resources.TryGetValue("LuckyPoint", out output);
                        BackgroundColor = ((Color)output);
                        break;
                    }
                case Xamarin.Forms.OSAppTheme.Light:
                    {
                        BackgroundColor = Color.White;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            await splashImage.ScaleTo(1, 2000);
            await splashImage.ScaleTo(0.9, 1500, Easing.Linear);
            await splashImage.ScaleTo(1, 2000);
            await splashImage.FadeTo(0, 1000);
            if(!Application.Current.MainPage.GetType().Equals(typeof(AppShell)))
            Application.Current.MainPage = new AppShell();
        }

        

    }
}
