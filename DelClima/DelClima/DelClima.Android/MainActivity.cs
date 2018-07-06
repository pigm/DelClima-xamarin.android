using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Json;
using System.IO;
using System;
using System.Net;
using Android.Content;

namespace DelClima.Droid
{
    [Activity(Label = "DelClima", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        EditText latitude;
        EditText longitude;
        Button button;
        Button button2;
        TextView location;
        TextView temperature;
        TextView humidity;
        TextView conditions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.Main);

             latitude = FindViewById<EditText>(Resource.Id.latText);
             longitude = FindViewById<EditText>(Resource.Id.longText);
             button = FindViewById<Button>(Resource.Id.btnVerClima);
             button2 = FindViewById<Button>(Resource.Id.btnLimpiarApp);

            button.Click += async (sender, e) =>
            {

                string url = "http://api.geonames.org/findNearByWeatherJSON?lat=" +
                             latitude.Text +
                             "&lng=" +
                             longitude.Text +
                             "&username=pablo_garrido";


                    JsonValue json = await FetchWeatherAsync(url);
                    ParseAndDisplay(json);
                
            };

            button2.Click += Button2_Click;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            latitude.Text = "-33.4446479";
            longitude.Text = "-70.6582719";
            location.Text = "";
            temperature.Text = "";
            humidity.Text = "";
            conditions.Text = "";
        }

        private async Task<JsonValue> FetchWeatherAsync(string url)
        {            
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
         
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }      

        private void ParseAndDisplay(JsonValue json)
        {
             location = FindViewById<TextView>(Resource.Id.lblUbicacion);
             temperature = FindViewById<TextView>(Resource.Id.lblTemperatura);
             humidity = FindViewById<TextView>(Resource.Id.lblHumedad);
             conditions = FindViewById<TextView>(Resource.Id.lblCondiciones);
         
            JsonValue weatherResults = json["weatherObservation"];

            location.Text = weatherResults["stationName"];

            double temp = weatherResults["temperature"];

            temp = ((9.0 / 5.0) * temp) + 32;
            temp = ((temp - 32) * (5.0 / 9.0));

            temperature.Text = String.Format("{0:F1}", temp) + "° C";

            double humidPercent = weatherResults["humidity"];
            humidity.Text = humidPercent.ToString() + "%";

             string cloudy = weatherResults["clouds"];
             if (cloudy.Equals("n/a"))
                 cloudy = "";
             if (cloudy.Equals("scattered clouds"))
                cloudy = "nubes dispersas";
            if (cloudy.Equals("clouds and visibility OK"))
                cloudy = "nubes y visibilidad Ok";
            if (cloudy.Equals("broken clouds"))
                cloudy = "nubes rotas";

             string cond = weatherResults["weatherCondition"];

            if (cond.Equals("n/a"))
                  cond = "";
            
            

              conditions.Text = cloudy + " " + cond;
        
        }

    }
}

