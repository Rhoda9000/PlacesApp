using PlacesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlacesApp.Services.Interfaces
{
    public interface IPlacesService
    {
        Task<List<Place>> SearchPlaces(string query);
        Task<PlaceDetails> GetPlaceDetails(string placeId);
    }
}
