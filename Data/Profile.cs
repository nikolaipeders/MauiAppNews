using SQLite;

namespace MauiAppNews.Data;

public class Profile
{
    [PrimaryKey]
    public string Mail { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Pass { get; set; }
}

