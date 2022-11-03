using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace MauiAppNews.Data;

public class ArticleService
{
    public async Task<List<Article>> GetArticlesAsync(List<Favorite> favorites)
    {
        Debug.WriteLine("Task start");
        var value = default(List<Article>);

        List<Article> articles = new List<Article>();

        // Connection
        TcpClient tcpClnt = new TcpClient();

        // Iterate through the favorites to do code below for each group 
        foreach (Favorite fav in favorites)
        {
            // Get ID's
            List<string> IDs = new List<string>();
            IDs = GetHeaders(fav.Header);

            Debug.WriteLine("current header = " + fav.Header);

            // Iterate through the ID's to call the body of each ID 
            foreach (string ID in IDs)
            {
                if (ID.Count() < 2)
                {
                    break;
                }
                Debug.WriteLine("current ID = " + ID);
                try
                {
                    tcpClnt = new TcpClient();
                    tcpClnt.Connect("news.sunsite.dk", 119);
                }
                catch (Exception)
                {
                }
                while (tcpClnt.Connected)
                {
                    Thread.Sleep(10);
                    Stream stream = tcpClnt.GetStream();

                    try
                    {
                        // Talk with the server
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        StreamWriter sw = new StreamWriter(stream);
                        sw.AutoFlush = true;

                        sw.WriteLine("authinfo user niko914n@easv365.dk");
                        Thread.Sleep(10);
                        reader.ReadLine(); // read this line and move the caret to next line

                        sw.WriteLine("authinfo pass 2ec5fd");
                        Thread.Sleep(10);
                        reader.ReadLine(); // read this line and move the caret to next line

                        sw.WriteLine("group" + " " + fav.Header);
                        Thread.Sleep(10);
                        reader.ReadLine(); // read this line and move the caret to next line

                        sw.WriteLine("body" + " " + ID);
                        Thread.Sleep(10);
                        reader.ReadLine(); // read this line and move the caret to next line

                        Article a = new Article();

                        while (true) 
                        {
                            string line = reader.ReadLine();
                            Debug.WriteLine(line);

                            a.Header = ID;
                            a.Group = fav.Header;
                            a.Body += line;
                            a.Body += "\xA";

                            if (line == ".")
                            {
                                articles.Add(a);
                                break;
                            }
                        }

                        // Close the stream + reader + socket
                        reader.Close();
                        stream.Close();
                        tcpClnt.Close();

                        value = articles;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            Debug.WriteLine("Now shifting group!");
        }
        return value;
    }


    public List<string> GetHeaders(string group)
    {
        var value = default(List<string>);

        TcpClient tcpClnt = new TcpClient();

        List<string> IDs = new List<string>();

        try
        {
            tcpClnt.Connect("news.sunsite.dk", 119);
        }
        catch (Exception)
        {
        }
        while (tcpClnt.Connected)
        {
            Thread.Sleep(10);
            Stream stream = tcpClnt.GetStream();

            try
            {
                // Talk with the server
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter sw = new StreamWriter(stream);
                sw.AutoFlush = true;

                sw.WriteLine("authinfo user niko914n@easv365.dk");
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("authinfo pass 2ec5fd");
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("group" + " " + group);
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("listgroup" + " " + group);
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line
                reader.ReadLine(); // read this line and move the caret to next line

                int counter = 0; // to set max

                while (counter < 5)
                {
                    string line = reader.ReadLine();
                    IDs.Add(line);
                    if (line == ".")
                    {
                        break;
                    }
                    counter++;
                }

                // Close the stream + reader + socket
                reader.Close();
                stream.Close();
                tcpClnt.Close();

                value = IDs;
                return value;
            }
            catch (Exception)
            {
            }
        }
        return value;
    }

    public string GetBody(string ID, string group)
    {
        var value = default(string);

        TcpClient tcpClnt = new TcpClient();

        try
        {
            tcpClnt.Connect("news.sunsite.dk", 119);
        }
        catch (Exception)
        {
        }
        while (tcpClnt.Connected)
        {
            Thread.Sleep(10);
            Stream stream = tcpClnt.GetStream();

            try
            {
                // Talk with the server
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter sw = new StreamWriter(stream);
                sw.AutoFlush = true;

                sw.WriteLine("authinfo user niko914n@easv365.dk");
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("authinfo pass 2ec5fd");
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("group" + " " + group);
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("body" + " " + ID);
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                Article a = new Article();

                while (true)
                {
                    string line = reader.ReadLine();
                    Debug.WriteLine(line);

                    a.Header = ID;
                    a.Group = group;
                    a.Body += line;
                    a.Body += "\xA";

                    if (line == ".")
                    {
                        return a.Body;
                    }
                }

                // Close the stream + reader + socket
                reader.Close();
                stream.Close();
                tcpClnt.Close();

                value = a.Body;
            }
            catch (Exception)
            {
            }
        }
        return value;
    }
}


