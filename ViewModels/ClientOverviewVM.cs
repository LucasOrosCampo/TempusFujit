using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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
        DateTime MinDate { get; } = DateTime.UtcNow.AddMonths(-1);
        DateTime MaxDate { get; } = DateTime.UtcNow.AddMonths(1);

        int clientId;
        public int ClientId
        {
            get => clientId;
            set
            {
                clientId = value;
            }
        }
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
        }
    }
}
