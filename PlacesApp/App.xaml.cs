using PlacesApp.Views;

namespace PlacesApp
{
    public partial class App : Application
    {
        public App(IServiceProvider ServiceProvider)
        {
            InitializeComponent();

            MainPage = new NavigationPage(ServiceProvider.GetRequiredService<SearchPage>());
        }
    }
}
