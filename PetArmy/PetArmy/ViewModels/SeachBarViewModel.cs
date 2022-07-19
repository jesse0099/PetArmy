using PetArmy.Models;
using PetArmy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class SearchBarViewModel : BaseViewModel
    {

        #region Singleton

        public static SearchBarViewModel Instance = null;


        public static SearchBarViewModel GetInstance()
        {
            if (Instance == null)
            {
                Instance = new SearchBarViewModel();
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

        public SearchBarViewModel()
        {
            initCommands();
            initClass();
            getTags();
            get30Pets();
        }

        public void initCommands()
        {
            Search = new Command(search);
            GetTags = new Command(getTags);
        }

        public void initClass()
        {

        }


        #endregion


        #region Varaiables

        private BindingList<Mascota> petsWithTag;
        public BindingList<Mascota> PetsWithTag
        {
            get { return petsWithTag; }
            set { petsWithTag = value; OnPropertyChanged(); }
        }

        private Tag searchedTag;

        public Tag SearchedTag
        {
            get { return searchedTag; }
            set { searchedTag = value; OnPropertyChanged(); }
        }

        private IEnumerable<Tag> tags;

        public IEnumerable<Tag> Tags
        {
            get { return tags; }
            set { tags = value; OnPropertyChanged(); }
        }


        #endregion


        #region Commands and Functions


        public ICommand Search { get; set; }

        public async void search()
        {
            try
            {
                PetsWithTag = new BindingList<Mascota>(await GraphQLService.getPetsByTag(SearchedTag.id_tag.ToString()).ConfigureAwait(false));

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand GetTags { get; set; }

        public async void getTags()
        {
            try
            {
                Tags = await GraphQLService.getAllTags().ConfigureAwait(false);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async void get30Pets()
        {
            try
            {
                PetsWithTag = new BindingList<Mascota>(await GraphQLService.get30Pets().ConfigureAwait(false));

            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

    }
}
