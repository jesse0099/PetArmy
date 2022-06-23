using PetArmy.Models;
using PetArmy.Services;
using Resx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged Fields
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool _openPopUp = false;
        public bool OpenPopUp
        {
            get { return _openPopUp; }
            set { SetProperty(ref _openPopUp, value); }
        }

        string  _errorTitle = string.Empty;
        public string  ErrorTitle
        {
            get { return _errorTitle; }
            set { SetProperty(ref _errorTitle, value); }
        }

        string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
      
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public LocalizedString Version { get; } = new(() => string.Format(AppResources.Version, AppInfo.VersionString));
        public LocalizedString CurrentLanguage { get; set; }

        public BaseViewModel()
        {
            CurrentLanguage = new(GetCurrentLanguageName);
            ChangeLanguage();
        }
        List<(Func<string> name, string value)> languageMapping { get; } = new()
        {
            (() => AppResources.System, null),
            (() => AppResources.English, "en"),
            (() => AppResources.Spanish, "es"),
        };

        private string GetCurrentLanguageName()
        {
            var (knownName, _) = languageMapping.SingleOrDefault(m => m.value == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);
            return knownName != null ? knownName() : LocalizationResourceManager.Current.CurrentCulture.DisplayName;
        }

        void ChangeLanguage()
        {
            string selectedValue = languageMapping.Single(m => m.name() == CurrentLanguage.Localized).value;
            LocalizationResourceManager.Current.CurrentCulture = selectedValue == null ? CultureInfo.CurrentCulture : new CultureInfo(selectedValue);
        }
    }
}
