using System.Net.Sockets;
using System.Text;

namespace MauiAppNews.Data;

public class GroupService
{
    public async Task<List<Group>> GetGroupsAsync()
    {
        var value = default(List<Group>);

        TcpClient tcpClnt = new TcpClient();

        List<Group> groups = new List<Group>();

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

                sw.WriteLine("list");
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line
                reader.ReadLine(); // read this line and move the caret to next line

                while (true)
                {
                    string line = reader.ReadLine();

                    string[] tokens = line.Split(' ');
                    line = tokens[0];

                    Group g = new Group();
                    g.Title = line;
                    groups.Add(g);
                    if (line == ("."))
                    {
                        break;
                    }
                }

                // Close the stream + reader + socket
                reader.Close();
                stream.Close();
                tcpClnt.Close();

                value = groups;
                return value;
            }
            catch (Exception)
            {
            }
        }
        return value;
    }

    public string[] GetArticleIDs(string newsGroup)
    {
        TcpClient tcpClnt = new TcpClient();

        List<string> articleIDs = new List<string>();

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

                sw.WriteLine("listgroup" + " " + newsGroup);
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                while (true)
                {
                    string line = reader.ReadLine();
                    articleIDs.Add(line);
                    if (line == ("."))
                    {
                        break;
                    }
                }
                // Close the stream + reader + socket
                reader.Close();
                stream.Close();
                tcpClnt.Close();

                string[] articlesIDsArray = articleIDs.ToArray();
                return articlesIDsArray;

            }
            catch (Exception)
            {
            }
        }
        return new string[0];
    }
}



