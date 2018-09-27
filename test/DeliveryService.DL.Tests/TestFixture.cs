﻿using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace DeliveryService.DL.Tests
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly TestServer Server;
        private readonly HttpClient Client;


        public TestFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot($"..\\..\\..\\..\\..\\src\\DeliveryService.API\\")
                .UseStartup<TStartup>();

            Server = new TestServer(builder);
            Client = new HttpClient();
        }


        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}
