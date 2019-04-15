using Nancy;
using Nancy.Hosting.Self;
using System;

namespace TestDbApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var myHost = new MyHosting();
            //Стартуем наш хостинг
            myHost.StartHost();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
            myHost.StopHost();
        }
    }

    class MyHosting
    {
        private NancyHost host;
        private string uri;

        public MyHosting()
        {
            HostConfiguration hostConfigs = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
            uri = "http://localhost:8081";
            host = new NancyHost(new Uri(uri), new DefaultNancyBootstrapper(), hostConfigs);
        }
        public MyHosting(ushort tcpPortNumber)
        {
            HostConfiguration hostConfigs = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
            uri = "http://localhost:" + tcpPortNumber;
            host = new NancyHost(new Uri(uri), new DefaultNancyBootstrapper(), hostConfigs);
        }

        public void StartHost()
        {
            host.Start();
            Console.WriteLine("Running on " + uri);
        }

        public void StopHost()
        {
            host?.Dispose();
        }
    }
}
