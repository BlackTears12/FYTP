using FYTP.ViewModel;

namespace FYTP.View.Windows
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