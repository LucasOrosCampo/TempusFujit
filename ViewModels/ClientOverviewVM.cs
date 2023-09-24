using CommunityToolkit.Maui.Alerts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TempusFujit.Infra;
using TempusFujit.Models;

namespace TempusFujit.ViewModels
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public class ClientOverviewVM : INotifyPropertyChanged
    {
        readonly IDbContextFactory<DatabaseContext> dbFactory = Services.DbFactory;
        DateTime _newStartingDate;
        DateTime _newEndingDate;
        DateTime _minEndingDate;
        public DateTime MinDate { get; } = DateTime.UtcNow.AddMonths(-1);
        public DateTime MaxDate { get; } = DateTime.UtcNow.AddMonths(1);

        int clientId;
        public int ClientId
        {
            get => clientId;
            set
            {
                clientId = value;
                Client = GetClient();
            }
        }

        int hoursInSelectedMonth;
        public int HoursInSelectedMonth
        {
            get => hoursInSelectedMonth;
            set { hoursInSelectedMonth = value; }
        }
        DateTime selectedMonth = DateTime.UtcNow;
        public DateTime SelectedMonth { get => selectedMonth; set { selectedMonth = value; } }

        private Client GetClient()
        {
            using var db = dbFactory.CreateDbContext();
            return db.Clients.First(x => x.Id == clientId);
        }
        Client client;
        public Client Client { get => client; set { client = value; OnPropertyChanged(); } }
        public DateTime NewStartingDate
        {
            get => _newStartingDate;
            set
            {
                _newStartingDate = value;
                MinEndingDate = value;
                this.OnPropertyChanged(PropertyChanged);
            }
        }
        public DateTime NewEndingDate
        {
            get => _newEndingDate;
            set
            {
                _newEndingDate = value;
                this.OnPropertyChanged(PropertyChanged);
            }
        }
        public DateTime MinEndingDate
        {
            get => _minEndingDate;
            set
            {
                _minEndingDate = value;
                this.OnPropertyChanged(PropertyChanged);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateTimeEntry { get; set; }

        public ClientOverviewVM()
        {
            CreateTimeEntry = new Command(execute: () => createTimeEntry());
        }

        private void createTimeEntry()
        {
            using var db = dbFactory.CreateDbContext();
            var newTimeEntry = new TimeEntry
            {
                ClientId = ClientId,
                StartingTime = NewStartingDate,
                EndingTime = NewEndingDate,
                CreationDate = DateTime.UtcNow
            };
            db.TimeEntries.Add(newTimeEntry);
            db.SaveChanges();
            Snackbar.Make("El tiempo ha sido correctamente añadido al cliente de mama", duration: TimeSpan.FromSeconds(2)).Show();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
