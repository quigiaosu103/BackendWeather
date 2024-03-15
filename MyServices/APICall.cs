using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WeatherBackend.MyServices
{
    class APICall
    {
        static readonly HttpClient client = new HttpClient();
        private static string appId = "376a7ca78ae6108dfac01a9935a41af7";
        public static async Task GetCity(string cityName) 
        {
            var response = await GetResponseAsync($"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={appId}");
            if (response.IsSuccessStatusCode)
            {
               try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                   
                    var jsonData = JArray.Parse(responseContent);

                    foreach (var item in jsonData)
                    {
                        Model.City city = new Model.City(item["name"].ToString(), item["lon"].ToString(), item["lat"].ToString());
                        MyRefData.insertCity(city);
                    };

                }
                catch (Exception ex)
                {
                    MyRefData.setMess("err parse");
                }
            }
            else
            {
                MyRefData.setMess("err call");
            }
        }

        public static async Task GetWeather(ILogger<Worker> _logger, string lon, string lat)
        {
            var response = await GetResponseAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={appId}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonData = JObject.Parse(responseContent);
                    var weather = new
                    {
                        id = jsonData["id"],
                        name = jsonData["name"],
                        country = jsonData["sys"]["country"],
                        windSpeed = jsonData["wind"]["speed"],
                        temp = jsonData["main"]["temp"],
                        humidity = jsonData["main"]["humidity"],
                        cloud = jsonData["clouds"]["all"]
                    };
                    _logger.LogInformation($"Weather at {weather.name} \n{weather}");

                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error {ex}", ex.Message);
                }
            }
            else
            {
                _logger.LogInformation("Call fail");
            }
        }

        public static async Task<HttpResponseMessage> GetResponseAsync(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            return response;
        }
    }
}
