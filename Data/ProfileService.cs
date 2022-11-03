
using SQLite;
using System.Diagnostics;

namespace MauiAppNews.Data;
public class ProfileService
{
    string _dbPath;
    private SQLiteAsyncConnection conn;
    public ProfileService(string dbPath)
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
            await conn.CreateTableAsync<Profile>();
        }
    }
    public async Task<List<Profile>> GetProfilesAsync()
    {
        await InitAsync();
        Debug.WriteLine("Task done");
        return await conn.Table<Profile>().ToListAsync();
    }
    public async Task<Profile> CreateProfileAsync(Profile paramProfile)
    {
        await conn.InsertAsync(paramProfile);
        return paramProfile;
    }

    public async Task<Profile> UpdateProfileAsync(Profile paramProfile)
    {
        await conn.UpdateAsync(paramProfile);
        return paramProfile;
    }

    public async Task<Profile> DeleteProfileAsync(Profile paramProfile)
    {
        await conn.DeleteAsync(paramProfile);
        return paramProfile;
    }
}