using System.Threading;
using Android.App;
using Android.OS;

namespace ToDoApp
{
    [Activity(Label = "To Do Items", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Thread.Sleep(1000);
            StartActivity(typeof(MainActivity));
        }
    }
}