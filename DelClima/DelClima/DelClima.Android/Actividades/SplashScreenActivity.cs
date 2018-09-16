using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;

namespace DelClima.Droid.Actividades
{
    /// <summary>
    /// Splash screen activity.
    /// </summary>
    [Activity(Theme = "@style/ThemePrincipal", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashScreenActivity : Activity
    {
        Button btnIngresar;
        ImageView imgClimaPresentacion;
        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splash_activity);
            imgClimaPresentacion = FindViewById<ImageView>(Resource.Id.imgClimaPresentacion);
            btnIngresar = FindViewById<Button>(Resource.Id.btnIngresar);
            Glide.With(this).Load(Resource.Drawable.weather).Into(imgClimaPresentacion);
            btnIngresar.Click += delegate {
                Intent intentClimaActivity = new Intent(this, typeof(ClimaActivity));
                StartActivity(intentClimaActivity);
            };
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() =>
            {
                Task.Delay(10000);
            });

            startupWork.ContinueWith(t => {
                btnIngresar.Visibility = ViewStates.Visible;
            }, TaskScheduler.FromCurrentSynchronizationContext());
            startupWork.Start();
        }
    }
}
