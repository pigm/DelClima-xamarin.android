namespace DelClima.Droid.Properties
{
    /// <summary>
    /// Constantes.
    /// </summary>
    public class Constantes
    {
        public const string URL_ENCABEZADO = "http://api.geonames.org/findNearByWeatherJSON?lat=";
        public const string URL_LONGITUD = "&lng=";
        public const string URL_USER = "&username=pablo_garrido";
        public const string WS_CONTENTTYPE = "application/json";
        public const string WS_METHOD = "GET";
        public const string JSON_WEATHER_RESULTS = "weatherObservation";
        public const string JSON_STATION_NAME = "stationName";
        public const string JSON_TEMPERATURE = "temperature";
        public const string JSON_HUMIDITY = "humidity";
        public const string JSON_CLOUDS = "clouds";
        public const string JSON_WEATHER_CONDITION = "weatherCondition";
        public const string NA = "n/a";
        public const string GRADOS_CELCIUS = "° C";
        public const string FORMAT_TEMPERATURE = "{0:F1}";
        public const string PORCIENTO = "%";

        public const double NUEVE_PUNTO_CERO = 9.0;
        public const double CINCO_PUNTO_CERO = 5.0;
        public const int TREINTAYDOS = 32;
    }
}
