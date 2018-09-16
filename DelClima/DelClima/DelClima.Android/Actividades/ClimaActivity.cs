using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Json;
using System.IO;
using System;
using System.Net;
using DelClima.Droid.Properties;
using Android.Views;

namespace DelClima.Droid
{
    /// <summary>
    /// Clima activity.
    /// </summary>
    [Activity(Theme = "@style/ThemePrincipal", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ClimaActivity : Activity
    {
        Button button;
        EditText latitude;
        EditText longitude;
        LinearLayout llPanelDatosTiempo;
        ProgressDialog _progressDialog;
        TextView location;
        TextView temperature;
        TextView humidity;
        TextView conditions;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.clima_activity);
            latitude = FindViewById<EditText>(Resource.Id.latText);
            longitude = FindViewById<EditText>(Resource.Id.longText);
            llPanelDatosTiempo = FindViewById<LinearLayout>(Resource.Id.llPanelDatosTiempo);
            button = FindViewById<Button>(Resource.Id.btnVerClima);
            button.Click += async (sender, e) =>
            {
                string url = Constantes.URL_ENCABEZADO + latitude.Text + Constantes.URL_LONGITUD + longitude.Text + Constantes.URL_USER;
                JsonValue json = await FetchWeatherAsync(url);
                ParseAndDisplay(json);
                closeLoading();
                llPanelDatosTiempo.Visibility = ViewStates.Visible;
            };
        }

        /// <summary>
        /// Loading this instance.
        /// </summary>
        public void loading()
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progressDialog.SetTitle("Procesando...");
            _progressDialog.SetMessage("Por favor, espera un momento");
            _progressDialog.Show();
            _progressDialog.SetCancelable(false);
        }

        public void closeLoading()
        {
            _progressDialog.Dismiss();
        }

        /// <summary>
        /// Fetchs the weather async.
        /// </summary>
        /// <returns>The weather async.</returns>
        /// <param name="url">URL.</param>
        private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            loading();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = Constantes.WS_CONTENTTYPE;
            request.Method = Constantes.WS_METHOD;
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                    return jsonDoc;
                }
            }
        }      

        /// <summary>
        /// Parses the and display.
        /// </summary>
        /// <param name="json">Json.</param>
        private void ParseAndDisplay(JsonValue json)
        {
             location = FindViewById<TextView>(Resource.Id.lblUbicacion);
             temperature = FindViewById<TextView>(Resource.Id.lblTemperatura);
             humidity = FindViewById<TextView>(Resource.Id.lblHumedad);
             conditions = FindViewById<TextView>(Resource.Id.lblCondiciones);
         
            JsonValue weatherResults = json[Constantes.JSON_WEATHER_RESULTS];

            location.Text = weatherResults[Constantes.JSON_STATION_NAME];

            double temp = weatherResults[Constantes.JSON_TEMPERATURE];

            temp = ((Constantes.NUEVE_PUNTO_CERO / Constantes.CINCO_PUNTO_CERO) * temp) + Constantes.TREINTAYDOS;
            temp = ((temp - Constantes.TREINTAYDOS) * (Constantes.CINCO_PUNTO_CERO / Constantes.NUEVE_PUNTO_CERO));

            temperature.Text = String.Format(Constantes.FORMAT_TEMPERATURE, temp) + Constantes.GRADOS_CELCIUS;

            double humidPercent = weatherResults[Constantes.JSON_HUMIDITY];
            humidity.Text = humidPercent.ToString() + Constantes.PORCIENTO;

            string cloudy = weatherResults[Constantes.JSON_CLOUDS];
             if (cloudy.Equals(Constantes.NA))
                cloudy = string.Empty;
             if (cloudy.Equals("scattered clouds"))
                cloudy = "nubes dispersas";
            if (cloudy.Equals("clouds and visibility OK"))
                cloudy = "nubes y visibilidad Ok";
            if (cloudy.Equals("broken clouds"))
                cloudy = "nubes rotas";

            string cond = weatherResults[Constantes.JSON_WEATHER_CONDITION];

            if (cond.Equals(Constantes.NA))
                cond = string.Empty;
              conditions.Text = cloudy + " " + cond;     
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
          
        }

        /// <summary>
        /// Ons the back pressed.
        /// </summary>
        public override void OnBackPressed()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("El Tiempo");
            builder.SetIcon(Resource.Drawable.tiempo);
            builder.SetMessage("¿Está seguro que deseas salir de la app?");
            builder.SetPositiveButton("Aceptar", delegate {

            });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
        }
    }
}