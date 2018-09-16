using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace DelClima.Droid.Actividades
{
    /// <summary>
    /// Main activity.
    /// </summary>
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() =>
            {
                Task.Delay(10);
            });

            startupWork.ContinueWith(t => {
                Intent intentSplashScreenActivity = new Intent(this, typeof(SplashScreenActivity));
                StartActivity(intentSplashScreenActivity);
            }, TaskScheduler.FromCurrentSynchronizationContext()); 
            startupWork.Start();
        }
    }
}