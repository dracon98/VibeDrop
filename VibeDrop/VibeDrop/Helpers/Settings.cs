using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using VibeDrop.Services;

namespace VibeDrop.Helpers
{
    public static class Settings
    {
        //This class will get and set other properties
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        const string LastSyncKey = "last_sync";
        static readonly DateTime LastSyncDefault = DateTime.Now.AddDays(-30);


        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;

        const string LoginAttemptsKey = "login_attempts";
        const int LoginAttemptsDefault = 0;

        const string NeedsSyncKey = "needs_sync";
        const bool NeedsSyncDefault = true;

        const string HasSyncedDataKey = "has_synced";
        const bool HasSyncedDataDefault = false;

        #endregion


        public static string AuthToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AuthTokenKey, AuthTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AuthTokenKey, value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }

        public static bool IsLoggedIn
        {
            get
            {
                if (!AzureService.UseAuth)
                    return true;

                return !string.IsNullOrWhiteSpace(UserId);
            }
        }
    }
}
