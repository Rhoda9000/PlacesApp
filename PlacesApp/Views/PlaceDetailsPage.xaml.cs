using PlacesApp.ViewModels;

namespace PlacesApp.Views;

public partial class PlaceDetailsPage : ContentPage
{
    private readonly PlaceDetailsViewModel _viewModel;

    public PlaceDetailsPage(PlaceDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public async Task LoadPlaceDetails(string placeId)
    {
        await _viewModel.LoadPlaceDetails(placeId);
    }
}