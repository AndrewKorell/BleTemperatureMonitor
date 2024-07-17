using BleTempMonitor.ViewModel;

namespace BleTempMonitor.Views;

public partial class SensorDetailsPage : ContentPage
{
	public SensorDetailsPage(SensorDetailsViewModel sensorDetailsViewModel)
	{
        InitializeComponent();
		BindingContext = sensorDetailsViewModel;
	}
}