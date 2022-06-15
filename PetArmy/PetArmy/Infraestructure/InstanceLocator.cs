using PetArmy.ViewModels;

namespace PetArmy.Infraestructure
{
    public  class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
