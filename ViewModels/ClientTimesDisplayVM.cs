using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TempusFujit.Infra;
using TempusFujit.Models;

namespace TempusFujit.ViewModels
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public class ClientTimesDisplayVM : INotifyPropertyChanged
    {
        private readonly IDbContextFactory<DatabaseContext> dbFactory = Services.DbFactory;

        List<TimeEntryVM> _currentlyDisplayedTimeEntries;
        public List<TimeEntryVM> CurrentlyDisplayedTimeEntries
        {
            get => _currentlyDisplayedTimeEntries;
            set
            {
                _currentlyDisplayedTimeEntries = value;
                OnPropertyChanged();
            }
        }

        bool globalCheckbox;
        public bool GlobalCheckbox { get => globalCheckbox; set { globalCheckbox = value; applyGlobalCheckbox(); } }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DisplayTimeOfClient { get; set; }
        public ICommand DeleteSelected { get; set; }
        public ICommand SelectAll { get; set; }

        public ClientTimesDisplayVM()
        {
            CurrentlyDisplayedTimeEntries = new List<TimeEntryVM>();
            DisplayTimeOfClient = new Command<int>(execute: id => displayTimeOfClient(id));
            DeleteSelected = new Command(execute: () => deleteSelected());
            SelectAll = new Command(execute: () => selectAll());
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

        private void deleteSelected()
        {
            using var db = dbFactory.CreateDbContext();
            var idsToBeDeleted = CurrentlyDisplayedTimeEntries.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            var te = db.TimeEntries.Where(e => idsToBeDeleted.Contains(e.Id)).ExecuteDelete();
            displayTimeOfClient(ClientId);
        }

        void applyGlobalCheckbox()
        {
            var times = CurrentlyDisplayedTimeEntries;
            times.ForEach(x => x.IsChecked = GlobalCheckbox);
            CurrentlyDisplayedTimeEntries = times;
        }
    }
    public class TimeEntryVM : TimeEntry, INotifyPropertyChanged
    {
        bool isChecked = false;
        public string Duration { get; set; }
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                this.OnPropertyChanged(PropertyChanged);
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
            var durationSpan = te.EndingTime is null ? null : (te.EndingTime - te.StartingTime);
            Duration = durationSpan.HasValue ? $"{durationSpan.Value.Days} d {durationSpan.Value.Hours} h {durationSpan.Value.Minutes} m" : "";
        }
    }
}
