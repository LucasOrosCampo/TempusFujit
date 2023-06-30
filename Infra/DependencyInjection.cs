﻿using TempusFujit.ViewModels;

namespace TempusFujit.Infra
{
    public static class DependencyInjection
    {
        public static void RegisterRepositories(ref MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();
            
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();
            
            builder.Services.AddSingleton<ClientPageViewModel>();
            builder.Services.AddSingleton<ClientPage>();

            builder.Services.AddSingleton<LoginPage>();
        }
    }
}
