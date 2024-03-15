using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherBackend.Model
{
    internal class City
    {
        private string name;
        private string lot;
        private string lat;

        public City(string _name, string _lot, string _lat)
        {
            this.name = _name;
            this.lot = _lot;
            this.lat = _lat;
        }

        public string getLot()
        {
            return this.lot;
        }

        public string getLat()
        {
            return this.lat;
        }

        public string toString()
        {
            return $"City: [name: {this.name}, lot: {this.lot}, lat: {this.lat}]";
        }

    }
}
