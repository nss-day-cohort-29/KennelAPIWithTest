﻿using Microsoft.AspNetCore.Mvc.Testing;
using KennelAPI;
using System.Net.Http;
using Xunit;

namespace KennelAPI.Tests
{
    class APIClientProvider : IClassFixture<WebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; private set; }
        private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();

        public APIClientProvider()
        {
            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory?.Dispose();
            Client?.Dispose();
        }
    }
}