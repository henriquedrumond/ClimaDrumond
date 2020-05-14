using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Acr.UserDialogs;
using ClimaDrumond.Helpers;
using ClimaDrumond.JSON;
using ClimaDrumond.ViewModel;

namespace ClimaDrumond
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //I decided not create a new class with CONST variables because is this a simple RH test. So, in normal situation I created a new static
        //in Helpers directory.

        public static string ForecastMapEndpoint = "https://api.openweathermap.org/data/2.5/forecast";
        public static string OpenWeatherMapAPIKey = "3c44d7acab661f37aabe135e670ff899";

        RestWebService _rest = new RestWebService();

        List<Cities> _cities = new List<Cities>();

        List<List> _forecastlist;

        public MainPage()
        {
            InitializeComponent();

            this.GetJsonFile();
        }

        protected override void OnAppearing()
        {
            base.OnDisappearing();

            pckCityName.SelectedIndexChanged += PckCityName_SelectedIndexChanged;
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            pckCityName.SelectedIndexChanged -= PckCityName_SelectedIndexChanged;
        }


        private async void PckCityName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _name = pckCityName.SelectedIndex != -1 ? pckCityName.Items[pckCityName.SelectedIndex] : null;

            if (!string.IsNullOrWhiteSpace(_name))
            {
                //Loading if case the net is slow.
                using (UserDialogs.Instance.Loading("Processing...", null, null, true, MaskType.Black))
                {
                    try
                    {
                        var _forecasttmp = await _rest.GetWeatherForeCast(GenerateRequestForeCast(ForecastMapEndpoint, _name));

                        _forecastlist = new List<List>();

                        //add in general list.
                        for (int i = 0; i < 5; i++)
                        {
                            _forecastlist.Add(_forecasttmp.list[i]);
                        }

                        //I could do using ModelView method, but the task in Model View Class takes a long time to return using this. I think that task change to long await
                        //after update ObservableCollection. So, I added the Model View class in the project to demonstrate one example about my thinking and knowledge
                        rpdList.ItemsSource = _forecastlist;

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        await DisplayAlert("Error", "One error in your request, try again in fill minutes.", "Cancel");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "Is necessary selected one city.", "Cancel");
            }
        }

        //Other option are do this method using MVVM I put one example in this projecto to demonstrate my practice.
        async void GetJsonFile()
        {
            //The file is very big. So, I put in the project. If the file were small I get the informations using URL.
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;

            //The file is one EmbbededResource. Is too slow because the file is very big. 
            Stream stream = assembly.GetManifestResourceStream("ClimaDrumond.city.list.json");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                //JSON interpreted as array 
                var objResponse1 = JsonConvert.DeserializeObject<List<Cities>>(json);

                foreach(var list in objResponse1)
                {
                    pckCityName.Items.Add(list.name);
                }
            }
        }

        /// <summary>
        /// The method create a request
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>String</returns>
        string GenerateRequestForeCast(string endpoint, string city)
        {
            string requestUri = endpoint;
            requestUri += $"?q={city}";
            requestUri += $"&APPID={OpenWeatherMapAPIKey}";
            return requestUri;
        }
    }
}
