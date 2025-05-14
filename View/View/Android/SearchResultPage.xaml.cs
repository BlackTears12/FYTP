using FYTP.ViewModel;

namespace FYTP.View.Android;

public partial class SearchResultPage : ContentPage
{
    public SearchResultPage(SearchResultViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}