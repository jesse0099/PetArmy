using PetArmy.Models;
using PetArmy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool _openPopUp = false;

        public bool OpenPopUp
        {
            get { return _openPopUp; }
            set
            {
                _openPopUp = value;
                OnPropertyChanged();
            }
        }

        string  _errorTitle = string.Empty;

        public string  ErrorTitle
        {
            get { return _errorTitle; }
            set {
                _errorTitle = value;
                OnPropertyChanged();
            }
        }

        string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _errorTitle; }
            set
            {
                _errorTitle = value;
                OnPropertyChanged();
            }
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
    }
}
