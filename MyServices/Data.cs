using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherBackend.MyServices
{
     class MyRefData
    {
        private static string mess ="";
        private static List<Model.City> cities = new List<Model.City>();
        private static string[] cityNames = { "hanoi", "hue", "london", "manchester", "Beijing ", "Seoul", "Tokyo" , "Jakarta", "Luxor", "Paris" };
        
        public static void insertCity(Model.City city)
        {
               cities.Add(city);
        }

        public static List<Model.City> getListCityData()
        {
            return cities;
        }

        public static void getCityData(ILogger<Worker> logger)
        {
            foreach(var name in cityNames)
            {
                APICall.GetCity(name);
                logger.LogInformation("get city "+ name);
            }
        }

        public static int getCitiesNum()
        {
            return cities.Count();
        }



        public static void setMess(string _city)
        {
            mess = _city;
        }

        public static string getMess()
        {
            return mess;
        }
    }
}
