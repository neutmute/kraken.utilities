using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;
using Kraken.Core;
using Kraken.Core.Extensions;
using Common.Logging;

namespace Kraken.Core
{
    #region enum KrakenConfigLocation
    /// <summary>
    /// Describe where to pull a config setting from
    /// </summary>
    public enum KrakenConfigLocation
    {
        Unknown = 0,

        /// <summary>
        /// App.Config
        /// </summary>
        AppSettings = 1,

     
    }
    #endregion

    public static class KrakenConfig
    {
        #region Fields
        private static Dictionary<string, bool> _featureActivationCache = new Dictionary<string, bool>();
        private static Dictionary<string, DateTime> _featureActivationDateCache = new Dictionary<string, DateTime>();
        static private readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        [CodeCoverageExcluded]
        static KrakenConfig()
        {
            Initialise();
        }
        #endregion

        #region Static Methods
        public static void Initialise()
        {
            lock (_featureActivationCache)
            {
                _featureActivationCache = new Dictionary<string, bool>();
                _featureActivationDateCache = new Dictionary<string, DateTime>();
            }
        }

        /// <summary>
        /// Assumes the settingKey is in SystemSetting and 
        /// </summary>
        public static DateTime GetDateFeatureActivated(KrakenConfigLocation settingLocation, string settingKey)
        {
            DateTime returnValue = DateTime.MaxValue;
            lock (_featureActivationDateCache)
            {
                if (_featureActivationDateCache.ContainsKey(settingKey))
                {
                    returnValue = _featureActivationDateCache[settingKey];
                }
                else
                {

                    returnValue = GetSettingAsDateTime(settingLocation, settingKey);
                    _featureActivationDateCache.Add(settingKey, returnValue);
                    Log.Trace(m => m("Feature activation (Date) {0}={1:yyyy-MMM-dd HH:mm:ss}", settingKey, returnValue));
                }
            }
            return returnValue;
        }

        public static DateTime GetSettingAsDateTime(KrakenConfigLocation settingLocation, string settingKey)
        {
            switch (settingLocation)
            {
                case KrakenConfigLocation.AppSettings:
                    return GetAppSettingAsDateTime(settingKey);
                //case KrakenConfigLocation.SystemSetting:
                //    return GetDatabaseSettingAsDateTime(settingKey);
                default:
                    throw KrakenException.Create("SettingLocation {0} not supported", settingLocation);
            }
        }

        /// <summary>
        /// Helper to determine whether a feature is enabled or not yet.
        /// Enables deploys of in progress code when other components aren't ready yet - ie: Converga
        /// </summary>
        /// <remarks>
        /// Use some caching to prevent repeated database hits
        /// </remarks>
        public static bool IsFeatureActivated(KrakenConfigLocation settingLocation, string settingKey)
        {
            bool returnValue;
            lock (_featureActivationCache)
            {
                if (_featureActivationCache.ContainsKey(settingKey))
                {
                    returnValue = _featureActivationCache[settingKey];
                }
                else
                {
                    returnValue = GetDateFeatureActivated(settingLocation, settingKey) < SystemDate.Now;
                    _featureActivationCache.Add(settingKey, returnValue);
                    Log.Trace(m => m("Feature activation {0}={1}", settingKey, returnValue));
                }
            }
            return returnValue;
        }
        #endregion

        #region App settings area
        /// <summary>
        /// Indicate whether a config value exists or not
        /// </summary>
        public static bool HasAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key] != null;
        }


        //public static string GetAppSettingAsString(string settingKey, string cryptoKey)
        //{
        //    string cipherValue = GetAppSettingAsString(settingKey, true);
        //    string plainValue = new Cryptographer(cryptoKey).ToPlainText(cipherValue);
        //    return plainValue;
        //}

        /// <summary>
        /// Get setting from appsettings section in app.config 
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="throwIfMissing">Control whether an exception should be thrown if the value is missing</param>
        public static string GetAppSettingAsString(string settingKey, bool throwIfMissing = true)
        {
            bool isMissingSetting = ConfigurationManager.AppSettings[settingKey] == null;
            if (isMissingSetting && throwIfMissing)
            {
                throw new ArgumentException("Missing key/value pair in AppSettings section in app.config for " + settingKey);
            }
            return ConfigurationManager.AppSettings[settingKey];
        }

        public static T GetAppSettingAsEnum<T>(string settingKey) 
        {
            return Enumeration.FromValue<T>(GetAppSettingAsString(settingKey));
        }

        public static bool GetAppSettingAsBoolean(string key, bool defaultValue)
        {
            var nullable = GetAppSettingAsBoolean(key);
            if (nullable == null)
            {
                return defaultValue;
            }
            return nullable.Value;
        }

        /// <summary>
        /// Do not throw an exception if this doesn't exist.
        /// Often, the lack of existance means implicitly false - eg: log disabled in production
        /// </summary>
        public static bool? GetAppSettingAsBoolean(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                return null;
            }
            return Convert.ToBoolean(ConfigurationManager.AppSettings[key]);
        }

        /// <summary>
        /// Load an integer value from appsettings
        /// </summary>
        public static int GetAppSettingAsInteger(string key)
        {
            return Convert.ToInt32(GetAppSettingAsString(key));
        }

        public static DateTime GetAppSettingAsDateTime(string key)
        {
            return DateTime.Parse(GetAppSettingAsString(key));
        }


        /// <summary>
        /// Load an integer value from appsettings
        /// </summary>
        public static int GetAppSettingAsInteger(string key, int defaultIfMissing)
        {
            string value = GetAppSettingAsString(key, false);
            if (value == null)
            {
                return defaultIfMissing;
            }
            return Convert.ToInt32(value);
        }
        #endregion

  

        //#region Database persisted settings
        //public static string GetDatabaseSettingAsString(string key)
        //{
        //    return SystemSettingDao.Get(key, true);
        //}

        //public static int GetDatabaseSettingAsInteger(string key)
        //{
        //    return Convert.ToInt32(GetDatabaseSettingAsString(key));
        //}

        //public static DateTime GetDatabaseSettingAsDateTime(string key)
        //{
        //    return Convert.ToDateTime(GetDatabaseSettingAsString(key));
        //}

        //public static bool GetDatabaseSettingAsBoolean(string key)
        //{
        //    return Convert.ToBoolean(GetDatabaseSettingAsString(key));
        //}

        //public static void SetDatabaseSetting(string key, int value)
        //{
        //    SystemSettingDao.Set(key, value);
        //}

        //public static void SetDatabaseSetting(string key, string value)
        //{
        //    SystemSettingDao.Set(key, value);
        //}

        //public static void SetDatabaseSetting(string key, DateTime value)
        //{
        //    SystemSettingDao.Set(key, value.ToString("yyyy-MM-dd HH:mm:ss"));
        //}

        //public static void SetDatabaseSetting(string key, bool value)
        //{
        //    SystemSettingDao.Set(key, value);
        //}
        //#endregion
    }
}
