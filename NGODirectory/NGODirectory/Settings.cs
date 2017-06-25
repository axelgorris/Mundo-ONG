using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGODirectory
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        
        const string EnableNotificationsKey = "enableNotifications";
        private static readonly bool EnableNotificationsDefault = true;
        public static bool EnableNotifications
        {
            get { return AppSettings.GetValueOrDefault(EnableNotificationsKey, EnableNotificationsDefault); }
            set { AppSettings.AddOrUpdateValue(EnableNotificationsKey, value); }
        }        
    }
}