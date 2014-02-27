using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using NLog;

namespace Kraken.Core
{
    public class ApplicationMetadata
    {
        #region Fields
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties

        public string Name { get; internal set; }

        public string Version { get; internal set; }

        public BuildConfiguration BuildConfiguration { get; internal set; }

        public string ExePath { get; internal set; }

        public string ExeFolder { get; internal set; }

        public string UserName { get; internal set; }
        #endregion

        #region Methods

        public void LogStartup()
        {
            LogStartup(null, null);
        }

        public void LogStartup(string supplementFormat, params object[] supplementArgs)
        {
            var statusMessage = GetLogStartupMessage();

            if (!string.IsNullOrEmpty(supplementFormat))
            {
                statusMessage += string.Format(supplementFormat, supplementArgs);
            }

            Log.Info(statusMessage);
            Log.Info("Culture: " + GetCultureDebug());
            Log.Info("Clock: " + GetTimeDebug());
        }

        private string GetLogStartupMessage()
        {
            string statusMessage = String.Format(
                "{0} is running {3} {1} ({5}) in ProcessId={2} on OS={4} under {6}"
                , Environment.MachineName
                , Version
                , Process.GetCurrentProcess().Id   // Make it easier to find which service is smashing a server CPU in task manager
                , Name
                , Environment.OSVersion
                , BuildConfiguration.GetDisplayName()
                , UserName
                );

            if (!ExePath.Contains("ASP.NET"))
            {
                statusMessage += " from " + ExePath;
            }


            return statusMessage;
        }

        private static string GetTimeDebug()
        {
            const string timeFormat = "yyyy-MM-dd HH:mm:ss";
            var localZone = TimeZone.CurrentTimeZone;
            var localTime = DateTime.Now;

            var s = string.Format(
                "Utc={1}, Local={0}, UtcOffset={3}, TimeZone={2}"
                , localTime.ToString(timeFormat)
                , DateTime.UtcNow.ToString(timeFormat)
                , localZone.StandardName
                , localZone.GetUtcOffset(localTime));

            return s;
        }

        private static string GetCultureDebug()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
            string s = string.Format(
                "{0}({1},{2},{3}) Decimal Separator={4} Group Separator={5}"
                , currentCulture.EnglishName
                , currentCulture.TwoLetterISOLanguageName
                , currentCulture.ThreeLetterISOLanguageName
                , currentCulture.Name
                , currentCulture.NumberFormat.NumberDecimalSeparator
                , currentCulture.NumberFormat.NumberGroupSeparator);

            return s;
        }

        public override string ToString()
        {
            return GetLogStartupMessage();
        }

        #endregion

    }
}