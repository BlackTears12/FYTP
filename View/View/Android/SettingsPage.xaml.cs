using FYTP.ViewModel;

namespace FYTP.View.Android
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage(SettingsViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
		}
	}
}