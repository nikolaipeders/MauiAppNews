using SQLite;

namespace MauiAppNews.Data;

public class Favorite
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string Header { get; set; }
}
