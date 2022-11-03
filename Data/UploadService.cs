using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace MauiAppNews.Data;

public class UploadService
{
    public string UploadArticle(string Header, string Body, string Group, string Author)
    {
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
                Thread.Sleep(100);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("authinfo pass 2ec5fd");
                Thread.Sleep(100);
                reader.ReadLine(); // read this line and move the caret to next line

                sw.WriteLine("post");
                Thread.Sleep(10);
                reader.ReadLine(); // read this line and move the caret to next line

                Debug.WriteLine(Header);
                Debug.WriteLine(Author);
                Debug.WriteLine(Group);
                Debug.WriteLine(Body);

                sw.WriteLine("from: " + Header);
                sw.WriteLine("subject: " + Author);
                sw.WriteLine("newsgroup: " + Group);
                sw.WriteLine();
                sw.WriteLine(Body);
                sw.WriteLine();
                sw.WriteLine(".");

                var response = reader.ReadLine();

                // Close the stream + reader + socket
                reader.Close();
                stream.Close();
                tcpClnt.Close();
                Debug.WriteLine(response);

                if (response.StartsWith("340"))
                {
                    return "Article posted!";
                }
                else
                {
                    return "Failed to post";
                }
                
            }
            catch (Exception)
            {
            }
        }
        return "Failed to post";
    }
}



