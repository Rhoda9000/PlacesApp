using PlacesApp.Model;
using PlacesApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace PlacesApp.ViewModels
{
    public class PlaceDetailsViewModel : BaseViewModel
    {
        private readonly IPlacesService _placesService;
        private PlaceDetails _placeDetails;
        private bool _isLoading;

        public PlaceDetails PlaceDetails
        {
            get => _placeDetails;
            set => SetProperty(ref _placeDetails, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand OpenUrlCommand { get; }


        public PlaceDetailsViewModel(IPlacesService placesService)
        {
            _placesService = placesService;
            OpenUrlCommand = new RelayCommand<string>(OpenUrl);
        }

        public async Task LoadPlaceDetails(string placeId)
        {
            try
            {
                IsLoading = true;
                PlaceDetails = await _placesService.GetPlaceDetails(placeId);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while loading place details.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void OpenUrl(string url)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    await Launcher.Default.OpenAsync(new Uri(url));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while trying to open the URL.", "OK");
            }
        }
    }
}
