using FFImageLoading.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlacesApp.Services.Data;
using PlacesApp.Services.Interfaces;
using PlacesApp.ViewModels;
using PlacesApp.Views;

namespace PlacesApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseFFImageLoading()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IPlacesService, PlacesService>();
            builder.Services.AddTransient<SearchViewModel>();
            builder.Services.AddTransient<SearchPage>();
            builder.Services.AddTransient<PlaceDetailsViewModel>();
            builder.Services.AddTransient<PlaceDetailsPage>();

            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Auth:ClientId", "94eb850d-448f-48ef-a085-5fee4807606e.apps.kerridgecs.com"},
                {"Auth:ClientSecret", "b609f130-2d13-43d4-93df-f8ab9f1cafde"}
            });


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
