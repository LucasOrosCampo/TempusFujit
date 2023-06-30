using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml;
using TempusFujit.Infra;
using TempusFujit.Models;

namespace TempusFujit.ViewModels
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public class ClientPageViewModel : INotifyPropertyChanged
    {
        List<TimeEntry> _currentlyDisplayedTimeEntries;
        DateTime _newStartingDate;
        DateTime _newEndingDate;
        DateTime _minEndingDate;
        private readonly IDbContextFactory<DatabaseContext> dbFactory =
            (IDbContextFactory<DatabaseContext>)MauiWinUIApplication.Current.Services.GetRequiredService(typeof(IDbContextFactory<DatabaseContext>));

        public List<TimeEntry> CurrentlyDisplayedTimeEntries
        {
            get => _currentlyDisplayedTimeEntries;
            set
            {
                _currentlyDisplayedTimeEntries = value;
                OnPropertyChanged();
            }
        }
        public DateTime NewStartingDate
        {
            get => _newStartingDate;
            set {
                _newStartingDate = value;
                MinEndingDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime NewEndingDate
        {
            get => _newEndingDate;
            set {
                _newEndingDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime MinEndingDate 
        {
            get => _minEndingDate;
            set
            {
                _minEndingDate = value;
                OnPropertyChanged();
            }
        }
        int clientId;
        public int ClientId
        {
            get => clientId;
            set
            {
                clientId = value;   
                displayTimeOfClient(value);
            }
        }
        public DateTime MinDate { get; }
        public DateTime MaxDate { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DisplayTimeOfClient { get; set; }
        public ICommand CreateTimeEntry { get; set; }

        public ClientPageViewModel()
        {
            CurrentlyDisplayedTimeEntries = new List<TimeEntry>();
            DisplayTimeOfClient = new Command<int>(execute: id => displayTimeOfClient(id));
            CreateTimeEntry = new Command(execute:() => createTimeEntry());
            MinDate = DateTime.UtcNow.AddMonths(-1);
            MaxDate = DateTime.UtcNow.AddMonths(1);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void displayTimeOfClient(int id)
        {
            using var db = dbFactory.CreateDbContext();
            CurrentlyDisplayedTimeEntries = db.TimeEntries.Where(x => x.ClientId == id).ToList(); 
        }

        private void createTimeEntry()
        {
            using var db = dbFactory.CreateDbContext();
            var newTimeEntry = new TimeEntry
            {
                ClientId = this.ClientId,
                StartingTime = NewStartingDate,
                EndingTime = NewEndingDate,
                CreationDate = DateTime.UtcNow
            };
            db.TimeEntries.Add(newTimeEntry);
            displayTimeOfClient(this.ClientId);
        }
    }
}
