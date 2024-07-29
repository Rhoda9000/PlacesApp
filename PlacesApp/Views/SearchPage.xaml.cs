using PlacesApp.Model;
using PlacesApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace PlacesApp.Views;

public partial class SearchPage : ContentPage
{
    private readonly SearchViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public SearchPage(SearchViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _serviceProvider = serviceProvider;
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _viewModel.IsLoading = true;
        if (e.SelectedItem is Place selectedPlace)
        {
            var placeDetailsPage = _serviceProvider.GetRequiredService<PlaceDetailsPage>();
            await placeDetailsPage.LoadPlaceDetails(selectedPlace.PlaceId);
            await Navigation.PushAsync(placeDetailsPage);
            ((ListView)sender).SelectedItem = null;
            _viewModel.IsLoading = false;
        }
    }
}
