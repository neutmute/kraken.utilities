using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Kraken.Core
{
    public class ApplicationMetadata
    {
        #region Properties

        public string Name { get; internal set; }

        public string Version { get; internal set; }

        public string InformationalVersion { get; internal set; }

        /// <summary>
        /// Can make some assert/decisions on this
        /// </summary>
        public BuildConfiguration BuildConfiguration { get; internal set; }

        /// <summary>
        /// For log rendering
        /// </summary>
        public string BuildConfigurationText { get; internal set; }

        public string ExePath { get; set; }

        public string ExeFolder { get; internal set; }

        public string UserName { get; internal set; }
        #endregion

        #region Methods

        internal string GetLogStartupMessage()
        {
            string statusMessage = String.Format(
                "{0} is running {3} {1} ({5}) in ProcessId={2} on OS={4} under {6}"
                , Environment.MachineName
                , Version
                , Process.GetCurrentProcess().Id   // Make it easier to find which service is smashing a server CPU in task manager
                , Name
                , Environment.OSVersion
                , BuildConfigurationText
                , UserName
                );

            if (!ExePath.Contains("ASP.NET") && !string.IsNullOrEmpty(ExePath))
            {
                statusMessage += " from " + ExePath;
            }

            return statusMessage;
        }

        public override string ToString()
        {
            return GetLogStartupMessage();
        }
        #endregion
    }
}