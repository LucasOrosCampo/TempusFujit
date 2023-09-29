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

        List<TimeEntryVM> allEntries;
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
                loadTimeEntries(value);
                FilterAndCompute();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DeleteSelected { get; set; }
        public ICommand SelectAll { get; set; }


        private void selectAll()
        {
            CurrentlyDisplayedTimeEntries.ForEach(x => x.IsChecked = true);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void loadTimeEntries(int id)
        {
            using var db = dbFactory.CreateDbContext();
            allEntries = db.TimeEntries.Where(x => x.ClientId == id).Include(x => x.Category).Select(x => new TimeEntryVM(x)).ToList();
            CurrentlyDisplayedTimeEntries = allEntries;
        }

        void updateTimeEntries(int id)
        {
            loadTimeEntries(id);
            filterDisplayedTimes();
        }

        private void deleteSelected()
        {
            using var db = dbFactory.CreateDbContext();
            var idsToBeDeleted = CurrentlyDisplayedTimeEntries.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            var te = db.TimeEntries.Where(e => idsToBeDeleted.Contains(e.Id)).ExecuteDelete();
            updateTimeEntries(ClientId);
        }

        void applyGlobalCheckbox()
        {
            var times = CurrentlyDisplayedTimeEntries;
            times.ForEach(x => x.IsChecked = GlobalCheckbox);
            CurrentlyDisplayedTimeEntries = times;
        }

        #region Filters
        DateTime startFilter = DateTime.Today;
        public DateTime StartFilter { get => startFilter; set { startFilter = value; FilterAndCompute(); } }
        bool startFilterActive;
        public bool StartFilterActive { get => startFilterActive; set { startFilterActive = value; FilterAndCompute(); } }

        DateTime endFilter = DateTime.Today;
        public DateTime EndFilter { get => endFilter; set { endFilter = value; FilterAndCompute(); } }
        bool endFilterActive;
        public bool EndFilterActive { get => endFilterActive; set { endFilterActive = value; FilterAndCompute(); } }


        Category selectedCategory;
        public Category SelectedCategory { get => selectedCategory; set { selectedCategory = value; FilterAndCompute(); } }
        bool categoryFilterActive;
        public bool CategoryFilterActive { get => categoryFilterActive; set { categoryFilterActive = value; FilterAndCompute(); } }


        List<Category> allCategories;
        public List<Category> AllCategories { get => allCategories; set { allCategories = value; OnPropertyChanged(); } }
        public void loadAllCategories()
        {
            using var db = dbFactory.CreateDbContext();
            AllCategories = db.Categories.ToList();
        }

        TimeSpan totalDuration;
        public TimeSpan TotalDuration { get => totalDuration; set { totalDuration = value; OnPropertyChanged(); } }

        public void FilterAndCompute()
        {
            filterDisplayedTimes();
            computeTotalDuration();
        }

        void computeTotalDuration()
        {
            TotalDuration = CurrentlyDisplayedTimeEntries.Aggregate(TimeSpan.Zero, (acc, x) => acc = acc + (x.EndingTime - x.StartingTime));
        }

        void filterDisplayedTimes()
        {
            var newCurrentlyDisplayed = allEntries;
            if (StartFilterActive)
                newCurrentlyDisplayed = newCurrentlyDisplayed.Where(x => x.StartingTime >= StartFilter.Date).ToList();
            if (EndFilterActive)
                newCurrentlyDisplayed = newCurrentlyDisplayed.Where(x => x.EndingTime <= EndFilter.Date.AddDays(1).AddTicks(-1)).ToList();
            if (CategoryFilterActive && SelectedCategory != null)
                newCurrentlyDisplayed = newCurrentlyDisplayed.Where(x => x.Category.Id == SelectedCategory.Id).ToList();
            CurrentlyDisplayedTimeEntries = newCurrentlyDisplayed;
        }

        #endregion
        public ClientTimesDisplayVM()
        {
            CurrentlyDisplayedTimeEntries = new List<TimeEntryVM>();
            DeleteSelected = new Command(execute: () => deleteSelected());
            SelectAll = new Command(execute: () => selectAll());
        }
    }
    public class TimeEntryVM : TimeEntry, INotifyPropertyChanged
    {
        public TimeSpan Duration { get; set; }

        bool isChecked = false;
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
            Category = te.Category;
            Duration = te.EndingTime - te.StartingTime;
        }
    }
}
