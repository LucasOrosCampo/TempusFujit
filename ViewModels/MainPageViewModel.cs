using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TempusFujit.Infra;
using TempusFujit.Models;

namespace TempusFujit.ViewModels
{
    public partial class MainPageViewModel : INotifyPropertyChanged

    {
        string _newClientName;
        string _newClientDescription;
        List<Client> _clients;
        List<Client> Clients { get => _clients; set { _clients = value; UpdateDisplayedClientList(); } }
        
        List<Client> _displayedClients;
        private readonly IDbContextFactory<DatabaseContext> dbFactory = 
            (IDbContextFactory<DatabaseContext>)MauiWinUIApplication.Current.Services.GetRequiredService(typeof(IDbContextFactory<DatabaseContext>));

        string _searchedTerm;
        public List<Client> DisplayedClients { get => _displayedClients; set { _displayedClients = value; OnPropertyChanged(); } }

        public string NewClientName {
            get => _newClientName;
            set 
            {
                _newClientName = value;
                OnPropertyChanged();
            }
        }
        public string NewClientDescription
        {
            get => _newClientDescription;
            set
            {
                _newClientDescription = value;
                OnPropertyChanged();
            }
        }

        public string SearchedTerm
        {
            get => _searchedTerm;
            set
            {
                _searchedTerm = value;
                OnPropertyChanged();
                UpdateDisplayedClientList();
            }
        }

        public ICommand AddClientCmd { get; set; }
        public ICommand RemoveClientCmd { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            using var db = dbFactory.CreateDbContext();
            Clients = db.Clients.ToList();

            AddClientCmd = new Command(execute: obj => AddClient());
            RemoveClientCmd = new Command<int>(execute: id => RemoveClient(id));
        }

        private async void AddClient()
        {
            using var db = dbFactory.CreateDbContext();
            if (!CanAddClient())
                return;
            var newClient = new Client(NewClientName, NewClientDescription);
            db.Clients.Add(newClient);
            if (db.SaveChanges()>0)
            {
                CleanNewClientFields();
                Clients = db.Clients.ToList();
            }
        }

        private bool CanAddClient() 
        {
            return NewClientName != null && NewClientName != "";
        }
        
        private async void RemoveClient(int id)
        {
            using var db = dbFactory.CreateDbContext();
            var clientToDelete = db.Clients.First(x => x.Id == id);
            db.Remove(clientToDelete);
            if (db.SaveChanges() > 0)
            {
                Clients = db.Clients.ToList();
            }

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateDisplayedClientList()
        {
            if (SearchedTerm != null)
            {
                DisplayedClients = Clients.Where(x => x.Name.Contains(SearchedTerm)).ToList();
            }
            else { DisplayedClients = Clients; }
        }

        private void CleanNewClientFields()
        {
            NewClientName = "";
            NewClientDescription = "";    
        }

    }


}
