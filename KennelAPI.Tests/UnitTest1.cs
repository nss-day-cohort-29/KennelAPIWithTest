using KennelAPI.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace KennelAPI.Tests
{
    public class StudentsControllerTests
    {
        [Fact]
        public async Task Get_All_Students_Returns_Some_Students()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/animals");

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
