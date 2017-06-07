using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace NGODirectory.Backend.Helpers
{
    public class HubNotificationHelper
    {
        HttpConfiguration _config;
        NotificationHubClient _hub;

        public HubNotificationHelper(HttpConfiguration config)
        {
            _config = config;

            // Obtenemos la configuración del proyecto del servidor
            MobileAppSettingsDictionary settings = _config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Obtenemos las credenciales del Notification Hubs de la Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Creamos un nuevo cliente Notification Hub.
            _hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);
        }

        public async Task<bool> SendPushNotification(string message)
        {
            // Enviamos el mensaje. Esto incluye APNS, GCM, WNS, y MPNS.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = message;

            try
            {
                // Enviamos la notificacion y obenemos el resultado
                var result = await _hub.SendTemplateNotificationAsync(templateParams);

                // Escribimos el resultado en los logs.
                _config.Services.GetTraceWriter().Info(result.State.ToString());

                return true;
            }
            catch (Exception ex)
            {
                // Escribimos el fallo en los logs.
                _config.Services.GetTraceWriter()
                .Error(ex.Message, null, "SendPushNotification Error");

                return false;
            }
        }
    }
}