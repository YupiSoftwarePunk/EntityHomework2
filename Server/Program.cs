using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Запуск HTTP сервера...");
            HttpServer.Start();

            await Task.Delay(-1);
        }
    }
}
