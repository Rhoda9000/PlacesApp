using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PlacesApp.Model;
using PlacesApp.Services.Interfaces;

namespace PlacesApp.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly IPlacesService _placesService;
        private string _searchQuery;
        private ObservableCollection<Place> _searchResults;
        private bool _isLoading;

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public ObservableCollection<Place> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand SearchCommand { get; }

        public SearchViewModel(IPlacesService placesService)
        {
            _placesService = placesService;
            SearchResults = new ObservableCollection<Place>();
            SearchCommand = new Command(async () => await PerformSearch());
        }

        private async Task PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            try
            {
                IsLoading = true;
                var results = await _placesService.SearchPlaces(SearchQuery);
                SearchResults.Clear();
                foreach (var result in results)
                {
                    SearchResults.Add(result);
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while searching for places.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
