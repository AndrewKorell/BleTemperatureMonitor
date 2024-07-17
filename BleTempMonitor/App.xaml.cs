using BleTempMonitor.Services;

namespace BleTempMonitor
{
    public partial class App : Application
    {

        private static IServiceProvider ServicesProvider;
        public static IServiceProvider Services => ServicesProvider;

        private static IAlertService AlertService;
        public static IAlertService AlertSvc => AlertService;

        private static ISettingsService SettingsService;
        public static ISettingsService Settings => SettingsService;

        private static ISensorStorageService SensorStorageService;
        public static ISensorStorageService SensorStorage => SensorStorageService;

        public readonly static LogService Logger = new();

        public App(IServiceProvider provider)
        {
            InitializeComponent();

            ServicesProvider = provider;
            AlertService = Services.GetService<IAlertService>();
            SettingsService = Services.GetService<ISettingsService>();
            SensorStorageService = Services.GetService<ISensorStorageService>();

            MainPage = new AppShell();
        }
    }
}
