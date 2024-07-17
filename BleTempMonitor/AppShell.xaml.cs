using BleTempMonitor.Views;

namespace BleTempMonitor
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SensorDetailsPage), typeof(SensorDetailsPage));
            //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
