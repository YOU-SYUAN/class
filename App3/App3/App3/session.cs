using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace App3
{
    public class session
    {
        private static ISettings AppSettings => CrossSettings.Current;

        // DisplayLanguage
        private const string UserKey = "User_key";

        public static string DisplayUserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserKey, null);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserKey, value);
            }
        }
    }
}
