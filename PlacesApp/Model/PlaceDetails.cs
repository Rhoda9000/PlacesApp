using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlacesApp.Model
{
    public class PlaceDetails
    {
        public string Name { get; set; }
        public string FormattedAddress { get; set; }
        public string Vicinity { get; set; }
        public string Url { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int UtcOffset { get; set; }
        public string PlaceId { get; set; }

        public List<Photo> Photos { get; set; }
    }
}
