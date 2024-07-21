using BleTempMonitor.Services;
using BleTempMonitor.ViewModel;
using BleTempMonitor.Views;
using Microsoft.Extensions.Logging;

namespace BleTempMonitor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<ISensorStorageService, SensorStorageService>();

           // string dbPath = Path.Combine(FileSystem.AppDataDirectory, "sensors.db3");
            builder.Services.AddSingleton<SensorDatabaseService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            //builder.Services.AddSingleton<BleService>();

            builder.Services.AddSingleton<BleScanViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddTransient<SensorDetailsViewModel>();
            builder.Services.AddTransient<ShowLogDataViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddTransient<SensorDetailsPage>();
            builder.Services.AddTransient<SensorLogViewPage>();

            return builder.Build();
        }
    }
}
