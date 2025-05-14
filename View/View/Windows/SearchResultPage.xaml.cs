using FYTP.ViewModel;

namespace FYTP.View.Windows;

public partial class SearchResultPage : ContentPage
{
	public SearchResultPage(SearchResultViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}