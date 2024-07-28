using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlacesApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetAccessTokenAsync();
    }
}
