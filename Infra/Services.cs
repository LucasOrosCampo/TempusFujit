using Microsoft.EntityFrameworkCore;

namespace TempusFujit.Infra
{
    public static class Services
    {
        public static object Get<T>()
        {
            return MauiWinUIApplication.Current.Services.GetRequiredService(typeof(T));
        }
        public static object GetDb()
        {
            return MauiWinUIApplication.Current.Services.GetRequiredService(typeof(IDbContextFactory<DatabaseContext>));
        }
    }
}
