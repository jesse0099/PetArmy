using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PetArmy.Helpers
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

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string GraphQLUrl = "https://pet-army-101.hasura.app/v1/graphql";
        private static readonly string URLDefault = "https://pet-army-101.hasura.app/v1/graphql";


        private const string GraphQLSecret = "sl0iQE5H49U1EtomqiRf43Wtq3YEXlPA0g2099PWuEiKMwb6qWn4r7od416BIrNn";
        private static readonly string SecretDefault = "sl0iQE5H49U1EtomqiRf43Wtq3YEXlPA0g2099PWuEiKMwb6qWn4r7od416BIrNn";

        #endregion


        public static string GQL_URL { get { return AppSettings.GetValueOrDefault(GraphQLUrl, URLDefault); } }

        public static string GQL_Secret { get { return AppSettings.GetValueOrDefault(GraphQLSecret, SecretDefault); } }


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }


        public static string UID
        {
            get
            {
                return AppSettings.GetValueOrDefault("UID", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("UID", value);
            }
        }

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault("Username", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Username", value);
            }
        }

        public static string Email
        {
            get
            {
                return AppSettings.GetValueOrDefault("Email", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Email", value);
            }

        }


        public static string Role
        {
            get
            {
                return AppSettings.GetValueOrDefault("Role", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Role", value);
            }

        }


        public static bool IsAdmin
        {
            get
            {
                return AppSettings.GetValueOrDefault("IsAdmin", true);
            }
            set
            {
                AppSettings.AddOrUpdateValue("IsAdmin", value);
            }

        }

        public static string Password
        {
            get { return AppSettings.GetValueOrDefault("Password", ""); }
            set { AppSettings.AddOrUpdateValue("Password", value); }
        }


    }
}
