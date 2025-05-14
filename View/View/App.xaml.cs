using Model;
using FYTP.ViewModel;

#if ANDROID
using FYTP.View.Android;
#else
using FYTP.View.Windows;
#endif

namespace FYTP.View
{
    public partial class App : Application
    {
        private MainViewModel viewModel;
        private ApplicationModel model;

        public App()
        {
            InitializeComponent();
            model = new ApplicationModel();
            viewModel = new MainViewModel(model);
            viewModel.PageOpened += ChangePage;
            MainPage = new NavigationPage(CreateMainPage(viewModel));
        }

        private void ChangePage(object? sender,Page page)
        {
            (MainPage as NavigationPage)?.PushAsync(page);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 450;
            const int newHeight = 600;

            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }

        public static Page CreateMainPage(ViewModel.MainViewModel mmv)
        {
            return new MainPage(mmv);
        }

        public static Page CreateSettingsPage(ViewModel.SettingsViewModel svm)
        {
            return new SettingsPage(svm);
        }

        public static Page CreateSearchResultPage(ViewModel.SearchResultViewModel vm)
        {
            return new SearchResultPage(vm);
        }
    }
}
