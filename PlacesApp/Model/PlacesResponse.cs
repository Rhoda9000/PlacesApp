using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlacesApp.Model
{
    public class PlacesResponse
    {
        public List<Place> Data { get; set; }
        public Meta Meta { get; set; }
    }
}
