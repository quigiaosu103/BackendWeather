using Naami.SuiNet.SuiTypes;
using Naami.SuiNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Naami.SuiNet.Types;
[DataContract]
public record Weather(
    [property: DataMember(Name = "id")] Uid Id,
    [property: DataMember(Name = "geoname_id")] string GeonameId,
    [property: DataMember(Name = "country")] string Country,
    [property: DataMember(Name = "latitude")] string Latitude,
    [property: DataMember(Name = "positive_latitude")] string PositiveLatitude,
    [property: DataMember(Name = "longitude")] string Longtitive,
    [property: DataMember(Name = "positive_longitude")] string PositiveLongtitude,
    [property: DataMember(Name = "weather_id")] string WeatherId,
    [property: DataMember(Name = "temp")] int Temp,
    [property: DataMember(Name = "pressure")] int Pressure,
    [property: DataMember(Name = "humidity")] int Humidity,
    [property: DataMember(Name = "visibility")] int Visibility,
    [property: DataMember(Name = "wind_deg")] int WindDeg,
    [property: DataMember(Name = "wind_gust")] int WindGust,
    [property: DataMember(Name = "clouds")] int Cloud,
   
    SuiObjectField<Attribute>[] Attributes
);