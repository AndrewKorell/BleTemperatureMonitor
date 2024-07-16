
using BleTempMonitor.ViewModel;

namespace BleTempMonitor
{
    public partial class MainPage : ContentPage
    {
        public MainPage(BleScanViewModel sensorNodeViewModel)
        {
            InitializeComponent();
            BindingContext = sensorNodeViewModel;
        }


    }
}
