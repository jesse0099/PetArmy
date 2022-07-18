using PetArmy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using PetArmy.Helpers;
using PetArmy.Services;
using PetArmy.Interfaces;

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

        private List<Mascota> petsWithTag;
        public List<Mascota> PetsWithTag
        {
            get { return petsWithTag; }
            set { petsWithTag = value; OnPropertyChanged(); }
        }

        private string searchedTag;

        public string SearchedTag
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
                petsWithTag = await GraphQLService.getPetsByTag(SearchedTag).ConfigureAwait(false);

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
                tags = await GraphQLService.getAllTags().ConfigureAwait(false);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void get30Pets()
        {
            try
            {
                petsWithTag = await GraphQLService.get30Pets().ConfigureAwait(false);

            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

    }
}
