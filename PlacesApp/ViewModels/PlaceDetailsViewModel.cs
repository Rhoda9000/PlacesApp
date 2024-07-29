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
using System.Collections.ObjectModel;

namespace PlacesApp.ViewModels
{
    public class PlaceDetailsViewModel : BaseViewModel
    {
        private readonly IPlacesService _placesService;
        private PlaceDetails _placeDetails;
        private bool _isLoading;
        private ImageSource _currentPhoto;
        private int _currentPhotoIndex;
        private bool _isLoadingPhoto;
        private ObservableCollection<PlaceDetails> _nearbyPlaces;
        private PlaceDetails _selectedNearbyPlace;

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

        public ImageSource CurrentPhoto
        {
            get => _currentPhoto;
            set => SetProperty(ref _currentPhoto, value);
        }

        public ObservableCollection<PlaceDetails> NearbyPlaces
        {
            get => _nearbyPlaces;
            set => SetProperty(ref _nearbyPlaces, value);
        }

        public PlaceDetails SelectedNearbyPlace
        {
            get => _selectedNearbyPlace;
            set
            {
                if (SetProperty(ref _selectedNearbyPlace, value))
                {
                    // Handle the selection change
                    if (_selectedNearbyPlace != null)
                    {
                        LoadPlaceDetails(_selectedNearbyPlace.PlaceId);
                    }
                }
            }
        }

        public ICommand OpenUrlCommand { get; }
        public ICommand LoadNextPhotoCommand { get; }
        public ICommand LoadPreviousPhotoCommand { get; }
        public ICommand SelectNearbyPlaceCommand { get; }

        public bool IsLoadingPhoto
        {
            get => _isLoadingPhoto;
            set => SetProperty(ref _isLoadingPhoto, value);
        }

        public PlaceDetailsViewModel(IPlacesService placesService)
        {
            _placesService = placesService;
            OpenUrlCommand = new RelayCommand<string>(OpenUrl);
            LoadNextPhotoCommand = new AsyncRelayCommand(LoadNextPhoto);
            LoadPreviousPhotoCommand = new AsyncRelayCommand(LoadPreviousPhoto);
            SelectNearbyPlaceCommand = new AsyncRelayCommand<PlaceDetails>(OnSelectNearbyPlace);
            _currentPhotoIndex = -1;
            NearbyPlaces = new ObservableCollection<PlaceDetails>();
        }

        public async Task LoadPlaceDetails(string placeId)
        {
            try
            {
                IsLoading = true;
                PlaceDetails = await _placesService.GetPlaceDetails(placeId);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while loading place details.", "OK");
            }
            finally
            {
                IsLoading = false;
                await LoadNextPhoto();
                await LoadNearbyPlaces();
            }
        }

        public async Task LoadNearbyPlaces()
        {
            try
            {
                var nearbyPlaces = await _placesService.FindNearbyPlaces(PlaceDetails.Latitude,PlaceDetails.Longitude);
                foreach (var place in nearbyPlaces)
                {
                    NearbyPlaces.Add(place);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading nearby places: {ex.Message}");
            }
        }

        private async Task OnSelectNearbyPlace(PlaceDetails selectedPlace)
        {
            if (selectedPlace != null)
            {
                await LoadPlaceDetails(selectedPlace.PlaceId);
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
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while trying to open the URL.", "OK");
            }
        }

        private async Task LoadNextPhoto()
        {
            if (_isLoadingPhoto || PlaceDetails?.Photos == null || _currentPhotoIndex >= PlaceDetails.Photos.Count - 1)
                return;

            try
            {
                _isLoadingPhoto = true;
                _currentPhotoIndex++;
                await LoadPhoto(_currentPhotoIndex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading photo: {ex.Message}");
            }
            finally
            {
                _isLoadingPhoto = false;
            }
        }

        private async Task LoadPreviousPhoto()
        {
            if (_isLoadingPhoto || PlaceDetails?.Photos == null || _currentPhotoIndex <= 0)
                return;

            try
            {
                _isLoadingPhoto = true;
                _currentPhotoIndex--;
                await LoadPhoto(_currentPhotoIndex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading photo: {ex.Message}");
            }
            finally
            {
                _isLoadingPhoto = false;
            }
        }

        private async Task LoadPhoto(int photoIndex)
        {
            if (photoIndex < 0 || photoIndex >= PlaceDetails.Photos.Count)
                return;

            var photo = PlaceDetails.Photos[photoIndex];
            var photoData = await _placesService.GetPhoto(photo.PhotoId);

            if (!string.IsNullOrEmpty(photoData))
            {
                var imageBytes = Convert.FromBase64String(photoData);
                var stream = new MemoryStream(imageBytes);
                CurrentPhoto = ImageSource.FromStream(() => stream);
            }
        }
    }
}