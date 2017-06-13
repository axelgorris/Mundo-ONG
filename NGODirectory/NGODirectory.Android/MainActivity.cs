using Android.App;
using Android.Content.PM;
using Android.OS;
using Gcm.Client;
using NGODirectory.Abstractions;
using NGODirectory.Droid.Services;
using Plugin.Permissions;
using System;
using Xamarin.Forms;

namespace NGODirectory.Droid
{
    [Activity(Label = "Mundo ONG", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static MainActivity instance = null;

        // Devuleve la activity actual
        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            // Asignamos la acutal instacia de MainAcivity
            instance = this;
            //Registramos las notificaciones
            RegisterPushNotifications();

            base.OnCreate(bundle);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            ((DroidPlatformProvider)DependencyService.Get<IPlatformProvider>()).Init(this);

            LoadApplication(new App());
        }

        private void RegisterPushNotifications()
        {
            try
            {
                // Validamos que todo este correcto para poder aplicar notificaciones Push
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);
                // Registramos el dispositivo para poder recibir notificaciones Push
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
        }

        private void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

