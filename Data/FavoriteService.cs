
using SQLite;
using System.Diagnostics;

namespace MauiAppNews.Data;
public class FavoriteService
{
    string _dbPath;
    private SQLiteAsyncConnection conn;
    public FavoriteService(string dbPath)
    {
        _dbPath = dbPath;
    }

    private async Task InitAsync()
    {
        // Don't Create database if it exists
        if (conn != null)
        {
            return;
        }
        else
        {
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Favorite>();
        }
    }
    public async Task<List<Favorite>> GetFavoritesAsync()
    {
        await InitAsync();
        Debug.WriteLine("Task done");
        return await conn.Table<Favorite>().ToListAsync();
    }
    public async Task<Favorite> CreateFavoriteAsync(Favorite paramFavorite)
    {
        await conn.InsertAsync(paramFavorite);
        return paramFavorite;
    }
    public async Task<Favorite> DeleteFavoriteAsync(Favorite paramFavorite)
    {
        await conn.DeleteAsync(paramFavorite);
        return paramFavorite;
    }
}