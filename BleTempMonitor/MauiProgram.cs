using BleTempMonitor.Services;
using BleTempMonitor.ViewModel;
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
#if DEBUG
            builder.Logging.AddDebug();
#endif
            //builder.Services.AddSingleton<BleService>();

            builder.Services.AddSingleton<SensorNodeViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<LoadingPage>();

            return builder.Build();
        }
    }
}
