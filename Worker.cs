using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WeatherBackend.Model;
using WeatherBackend.MyServices;

namespace WeatherBackend
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            MyRefData.getCityData(_logger);
            while (!stoppingToken.IsCancellationRequested)
            {
                if (MyRefData.getCitiesNum()>0)
                {
                    var citiesData = MyRefData.getListCityData();
                    foreach(Model.City city in citiesData)
                    {
                        if(MyRefData.getCitiesNum()>0)
                        {
                            APICall.GetWeather(_logger, city.getLot(), city.getLat());
                        }
                    }

                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
