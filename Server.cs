using System.Net;
using System.Text;

class Server
{
    private readonly string url = "http://localhost:8000/";

    public void Start()
    {
        HttpListener listener = new HttpListener();

        listener.Prefixes.Add(url);
        listener.Start();

        Console.WriteLine($"Listening for requests on {url}");

        ThreadPool.QueueUserWorkItem((obj) =>
        {
            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();

                HttpListenerResponse response = context.Response;

                string responseString = "Hello world!";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentType = "text/plain";
                response.ContentLength64 = buffer.Length;

                response.OutputStream.Write(buffer, 0, buffer.Length);

                response.OutputStream.Close();
            }
        });

        Console.WriteLine("Press Enter to stop the server");
        Console.ReadLine();

        listener.Stop();
    }
}