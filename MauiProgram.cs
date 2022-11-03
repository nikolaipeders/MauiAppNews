using Microsoft.AspNetCore.Components.WebView.Maui;
using MauiAppNews.Data;
using MatBlazor;

namespace MauiAppNews;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMatBlazor();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<ArticleService>();
        builder.Services.AddSingleton<GroupService>();
        builder.Services.AddSingleton<UploadService>();

        // Set path to the SQLite database (it will be created if it does not exist)
        var dbPathProfile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ProfileService.db");
        // Register FavoriteService and the SQLite database
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<ProfileService>(s, dbPathProfile));

        // Set path to the SQLite database (it will be created if it does not exist)
        var dbPathFavorite = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"FavoriteService.db");
        // Register FavoriteService and the SQLite database
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<FavoriteService>(s, dbPathFavorite));

        return builder.Build();
    }
}
