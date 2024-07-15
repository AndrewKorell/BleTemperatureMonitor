
using BleTempMonitor.ViewModel;

namespace BleTempMonitor
{
    public partial class MainPage : ContentPage
    {
        public MainPage(SensorNodeViewModel sensorNodeViewModel)
        {
            InitializeComponent();
            BindingContext = sensorNodeViewModel;
        }


    }
}
