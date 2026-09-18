using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using IngestionService.Exceptions;
using IngestionService.Models.Configurations;
using IngestionService.Models.DTOs;

namespace IngestionService.Services
{
    public class StationApiService
    {
        private HttpClient _httpClient;
        private StationsAPIs _stationURLs;

        public StationApiService(HttpClient httpClient, StationsAPIs stationURLs)
        {
            _httpClient = httpClient;
            _stationURLs = stationURLs;
        }

        public async Task<IEnumerable<StationInformationDTO>> GetStationsInformationAsync()
        {
            return _GetFromContent<StationInformationDTO>( await _GetFromURL(_stationURLs.StationInformationAPI), _stationURLs.StationInformationKeyPath);
        }
        public async Task<IEnumerable<StationStatusDTO>> GetStationStatusesAsync()
        {
            return _GetFromContent <StationStatusDTO>( await _GetFromURL(_stationURLs.StationStatusAPI), _stationURLs.StationStatusKeyPath);
        }
        public async Task<IEnumerable<VehicleTypesDTO>> GetVehicleTypesAsync()
        {
            return _GetFromContent<VehicleTypesDTO>(await _GetFromURL(_stationURLs.VehicleTypesAPI), _stationURLs.VehicleTypesKeyPath);
        }

        private string _GetContentFromJson(JsonDocument document, IEnumerable<string> KeyPath)
        {
            JsonElement target = document.RootElement;
            foreach (string key in KeyPath)
            {
                try
                {
                    target = target.GetProperty(key);
                }

                catch (KeyNotFoundException)
                {
                    throw new DeserializeException(target.ToString(), "", $"Error while accessing key path: {key} - Not exists");
                }

            }
            string content = target.ToString();

            return content;
        }
        private IEnumerable<T> _GetFromContent<T>(JsonDocument document, IEnumerable<string> KeyPath)
        {
            string content = _GetContentFromJson(document, KeyPath);
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    PropertyNameCaseInsensitive = true
                };
                List<T>? serializedData = JsonSerializer.Deserialize<List<T>>(content, options);
                if (serializedData == null)
                {
                    throw new DeserializeException(content, typeof(T).Name, "json Deserialize failed: return null");
                }
                return serializedData;
            }
            catch (JsonException ex)
            {
                throw new DeserializeException(content, typeof(T).Name, ex.Message);
            }
        }

        private async Task<JsonDocument> _GetFromURL(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseException(response.StatusCode,content);
                }
                else
                {
                    JsonDocument document = JsonDocument.Parse(content);
                    return document;
                }
            }
    
            catch(HttpRequestException ex)
            {
                throw new ApiConnectionException(url, ex.Message);
                
            }
            catch (InvalidOperationException ex)
            {
                throw new ApiConnectionException(url, ex.Message);
            }
        }
    }
}