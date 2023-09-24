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

        #region Client
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

        Client client;
        public Client Client { get => client; set { client = value; OnPropertyChanged(); } }
        private Client GetClient()
        {
            using var db = dbFactory.CreateDbContext();
            return db.Clients.First(x => x.Id == clientId);
        }
        #endregion

        #region TimeEntryInput
        public DateTime TaskDate { get; set; }

        TimeSpan taskStartingTime = DateTime.Now.TimeOfDay;
        public TimeSpan TaskStartingTime
        {
            get => taskStartingTime;
            set { taskStartingTime = value; UpdateAddButton(value, TaskEndingTime); }
        }

        TimeSpan taskEndingTime = DateTime.Now.TimeOfDay;
        public TimeSpan TaskEndingTime
        {
            get => taskEndingTime;
            set { taskEndingTime = value; UpdateAddButton(TaskStartingTime, value); }
        }

        bool isAddButtonEnabled;
        public bool IsAddButtonEnabled { get => isAddButtonEnabled; set { isAddButtonEnabled = value; OnPropertyChanged(); } }

        private void UpdateAddButton(TimeSpan initial, TimeSpan final)
        {
            IsAddButtonEnabled = initial < final;
        }

        public ICommand CreateTimeEntry { get; set; }

        void createTimeEntry()
        {
            using var db = dbFactory.CreateDbContext();
            var newTimeEntry = new TimeEntry
            {
                ClientId = ClientId,
                CreationDate = DateTime.UtcNow,
                StartingTime = TaskDate.Date.Add(TaskStartingTime),
                EndingTime = TaskDate.Date.Add(TaskEndingTime)
            };
            db.TimeEntries.Add(newTimeEntry);
            db.SaveChanges();
            ComputeHours();
            Snackbar.Make("El tiempo ha sido correctamente añadido al cliente de mama", duration: TimeSpan.FromSeconds(2)).Show();
        }
        #endregion

        #region Selected month 
        TimeSpan timeSpentInSelectedPeriod;
        public TimeSpan TimeSpentInSelectedPeriod
        {
            get => timeSpentInSelectedPeriod;
            set { timeSpentInSelectedPeriod = value; OnPropertyChanged(); }
        }
        DateTime selectedMonth = DateTime.Now;
        public DateTime SelectedMonth { get => selectedMonth; set { selectedMonth = value; ComputeHours(); } }

        public void ComputeHours()
        {
            var month = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1).Date;
            var nextMonth = month.AddMonths(1).AddTicks(-1);
            using var db = dbFactory.CreateDbContext();
            var timeEntriesInPeriod = db.TimeEntries.Where(x => x.StartingTime >= month && x.EndingTime <= nextMonth).ToList();
            TimeSpentInSelectedPeriod = timeEntriesInPeriod.Aggregate(TimeSpan.Zero, (acc, x) => acc + ((TimeSpan)(x.EndingTime - x.StartingTime)));
        }
        #endregion

        #region Property changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ClientOverviewVM()
        {
            TaskDate = DateTime.UtcNow;
            CreateTimeEntry = new Command(execute: () => createTimeEntry());
        }
    }
}
