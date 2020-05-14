using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClimaDrumond.JSON;

namespace ClimaDrumond.ViewModel
{
    public class ForeCastViewModel
    {
        protected HttpClient _client;

        public ObservableCollection<List> _forecastlist { get; } = new ObservableCollection<List>();

        public ForeCastViewModel(string query)
        {
            this.GetWeatherForeCast(query);
        }

        public async void GetWeatherForeCast(string query)
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
        }
    }
}
