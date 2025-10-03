using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Client? client;
        
        public bool IsAdmin => Client?.Role == Role.Admin;
        
        public void SetCurrentClient(Client client)
        {
            Client = client;
            OnPropertyChanged(nameof(IsAdmin));
        }
        
        public void Logout()
        {
            Client = null;
            OnPropertyChanged(nameof(IsAdmin));
        }
    }
}