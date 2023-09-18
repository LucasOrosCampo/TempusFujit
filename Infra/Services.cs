using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TempusFujit.Infra
{
    public static class Services
    {
        public static object Get<T>()
        {
            return MauiWinUIApplication.Current.Services.GetRequiredService(typeof(T));
        }
        public static IDbContextFactory<DatabaseContext> DbFactory =>
             MauiWinUIApplication.Current.Services.GetRequiredService(typeof(IDbContextFactory<DatabaseContext>))
             as IDbContextFactory<DatabaseContext>;

        public static void OnPropertyChanged(this object caller, PropertyChangedEventHandler eh, [CallerMemberName] string propertyName = null)
        {
            eh?.Invoke(caller, new PropertyChangedEventArgs(propertyName));
        }
    }
}
