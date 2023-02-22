using System.Text;
using System.Text.Json;
using PaltformService.Dtos;
using PaltformService.SyncDataServices.Http.Interfaces;

namespace PaltformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(
            HttpClient httpClient, 
            IConfiguration configuration
        )
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                $"{_configuration["CommandServiceURL"]}",
                httpContent
            );

            if(response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("--> Sync POST to CommandService was OK");
            }
            else
            {
                System.Console.WriteLine("--> Sync POST to CommandService was NOT OK");
            }
        }
    }
}