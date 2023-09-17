using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TempusFujit.Infra;
using TempusFujit.Models;

namespace TempusFujit.ViewModels
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public class ClientPageViewModel : INotifyPropertyChanged
    {
        List<TimeEntryVM> _currentlyDisplayedTimeEntries;
        DateTime _newStartingDate;
        DateTime _newEndingDate;
        DateTime _minEndingDate;
        private readonly IDbContextFactory<DatabaseContext> dbFactory =
            (IDbContextFactory<DatabaseContext>)MauiWinUIApplication.Current.Services.GetRequiredService(typeof(IDbContextFactory<DatabaseContext>));

        public List<TimeEntryVM> CurrentlyDisplayedTimeEntries
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
            set
            {
                _newStartingDate = value;
                MinEndingDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime NewEndingDate
        {
            get => _newEndingDate;
            set
            {
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
        public ICommand DeleteSelected { get; set; }
        public ICommand SelectAll { get; set; }

        public ClientPageViewModel()
        {
            CurrentlyDisplayedTimeEntries = new List<TimeEntryVM>();
            DisplayTimeOfClient = new Command<int>(execute: id => displayTimeOfClient(id));
            CreateTimeEntry = new Command(execute: () => createTimeEntry());
            DeleteSelected = new Command(execute: () => deleteSelected());
            SelectAll = new Command(execute: () => selectAll());
            MinDate = DateTime.UtcNow.AddMonths(-1);
            MaxDate = DateTime.UtcNow.AddMonths(1);
        }

        private void selectAll()
        {
            CurrentlyDisplayedTimeEntries.ForEach(x => x.IsChecked = true);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void displayTimeOfClient(int id)
        {
            using var db = dbFactory.CreateDbContext();
            CurrentlyDisplayedTimeEntries = db.TimeEntries.Where(x => x.ClientId == id).Select(x => new TimeEntryVM(x)).ToList();
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
            displayTimeOfClient(ClientId);
        }

        private void deleteSelected()
        {
            using var db = dbFactory.CreateDbContext();
            var idsToBeDeleted = CurrentlyDisplayedTimeEntries.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            var te = db.TimeEntries.Where(e => idsToBeDeleted.Contains(e.Id)).ExecuteDelete();
            displayTimeOfClient(ClientId);
        }
    }
    public class TimeEntryVM : TimeEntry, INotifyPropertyChanged
    {
        bool isChecked = false;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TimeEntryVM(TimeEntry te)
        {
            ClientId = te.ClientId;
            Id = te.Id;
            Client = te.Client;
            CreationDate = te.CreationDate;
            StartingTime = te.StartingTime;
            EndingTime = te.EndingTime;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

