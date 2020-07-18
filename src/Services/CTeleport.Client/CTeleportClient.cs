using System;
using System.Net.Http;
using System.Threading.Tasks;
using CTeleport.Client.Interfaces;
using CTeleport.Client.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace CTeleport.Client
{
    public class CTeleportClient : ICTeleportClient
    {
        private readonly Settings _settings;
        private readonly ILogger<CTeleportClient> _logger;
        private readonly IHttpClientFactory _clientFactory;

        private readonly ICache _cache;

        public CTeleportClient(IOptions<Settings> settings, ILogger<CTeleportClient> logger, IHttpClientFactory clientFactory,
            ICache cache)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _cache = cache;
        }

        public async Task<AirportInfo> GetAirportInfo(string iataCode)
        {
            if (string.IsNullOrWhiteSpace(iataCode) || iataCode.Length != 3) return null;

            var url = $"{_settings.Url}/airports/{iataCode}";

            var response = _cache != null ? await GetCachedData(url) : await GetDataAsync(url);

            return string.IsNullOrWhiteSpace(response) ? null : JsonConvert.DeserializeObject<AirportInfo>(response);
        }

        private async Task<string> GetCachedData(string url)
        {
            var response = _cache.GetCachedData(url);
            if (!string.IsNullOrWhiteSpace(response)) return response;

            response = await GetDataAsync(url);
            if (!string.IsNullOrWhiteSpace(response))
            {
                _cache.PutCachedData(url, response);
            }

            return response;
        }

        private async Task<string> GetDataAsync(string url)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                return await client.GetStringAsync(url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"CTeleport {url} invoked an error");
                return null;
            }
        }
    }
}
