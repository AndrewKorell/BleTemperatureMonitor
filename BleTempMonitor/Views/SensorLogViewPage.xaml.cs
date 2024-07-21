using BleTempMonitor.ViewModel;

namespace BleTempMonitor.Views;

public partial class SensorLogViewPage : ContentPage
{
	public SensorLogViewPage(ShowLogDataViewModel showLogDataViewModel)
	{
		InitializeComponent();
		BindingContext = showLogDataViewModel;
	}
}