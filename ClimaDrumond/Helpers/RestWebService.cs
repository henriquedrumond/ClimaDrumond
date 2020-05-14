using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClimaDrumond.JSON;

namespace ClimaDrumond.Helpers
{
    public class RestWebService
    {
        public List<List> _tmplist;
        protected HttpClient _client;

        public RestWebService()
        {
            _client = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherData(string query)
        {
            WeatherData weatherData = null;

            try
            {
                var response = await _client.GetAsync(query);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
            }

            return weatherData;
        }


        public async Task<RootObject> GetWeatherForeCast(string query)
        {
            RootObject weatherData = null;

            try
            {
                var response = await _client.GetAsync(query);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<RootObject>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
            }

            return weatherData;
        }
    }
}