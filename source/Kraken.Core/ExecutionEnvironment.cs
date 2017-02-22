using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

namespace Kraken.Core
{
    public static class ExecutionEnvironment
    {
        #region Fields
        static AssemblyCompilationAttribute _compilationAttribute;

        /// <summary>
        /// Profiling showed the determination of this was fairly expensive, so lets cache the value
        /// </summary>
        private static bool _isUnitTest;
        private static string _executionPath;
        #endregion

        #region Properties
        /// <summary>
        /// Indicate whether we are running in a unit test environment or not.
        /// </summary>
        /// <remarks>
        /// Should be used with restraint since it is dangerous to make code decisions based on whether we are in 
        /// unit test or not but used judiciously it is useful.
        /// </remarks>
        public static bool IsUnitTest
        {
            get { return _isUnitTest; }
        }

        /// <summary>
        /// Was the code compiled in DEBUG or RELEASE configuration
        /// </summary>
        /// <remarks>
        /// To use this you must mark all of your project assemblies with AssemblyCompilationAttribute in solution info
        /// See wiki for details
        /// 
        /// This attribute may be used to render developer versions of pages in websites - for example 
        /// Network Online shows the magic logon buttons when running DEBUG bits
        /// </remarks>
        public static bool IsDebug
        {
            get
            {
                EnsureCustomAttributeExists(Assembly.GetCallingAssembly());
                return _compilationAttribute.IsDebug;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Load the expensive data and save it
        /// </summary>
        static ExecutionEnvironment()
        {
            _executionPath = Assembly.GetExecutingAssembly().Location.ToLower();

            bool devTest = _executionPath.IndexOf("nunit") > -1;
            bool cruiseTest = _executionPath.IndexOf("unittests") > -1;

            _isUnitTest = devTest || cruiseTest;
        }
        #endregion

        #region Instance Methods
        public static ApplicationMetadata GetApplicationMetadata()
        {
            var appMetadata = new ApplicationMetadata();

            // If its ASP.NET, entry assenbly is null
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

            appMetadata.Name = assembly.GetName().Name;
            appMetadata.Version = assembly.GetName().Version.ToString();
            appMetadata.ExePath = assembly.Location;

            if (!string.IsNullOrEmpty(assembly.Location)) // vNext no assembly magic
            {
                appMetadata.ExeFolder = new FileInfo(assembly.Location).DirectoryName;
            }

            var infoVersionAttribute = GetCustomAttribute<AssemblyInformationalVersionAttribute>(assembly);
            if (infoVersionAttribute != null)
            {
                appMetadata.InformationalVersion = infoVersionAttribute.InformationalVersion;
            }

            appMetadata.UserName = Environment.UserName;

            var compilationAttribute = GetCustomAttribute<AssemblyCompilationAttribute>(assembly);
            if (compilationAttribute != null)
            {
                appMetadata.BuildConfiguration = compilationAttribute.BuildConfiguration;
                appMetadata.BuildConfigurationText = appMetadata.BuildConfiguration.ToString();
            }
            else
            {
                var configurationAttribute = GetCustomAttribute<AssemblyConfigurationAttribute>(assembly);
                appMetadata.BuildConfiguration = BuildConfiguration.Unknown;
                if (configurationAttribute != null)
                {
                    appMetadata.BuildConfigurationText = configurationAttribute.Configuration;
                }
                else
                {
                    appMetadata.BuildConfigurationText = "Unknown";
                }
            }

            return appMetadata;
        }

        /// <summary>
        /// Throws a NotSupportedException if we are not in a unit test
        /// </summary>
        /// <remarks>By having this in the library, we don't have to mark the consumer method as CodeCoverageExcluded for unit tests</remarks>
        [CodeCoverageExcluded("Cannot cover this in a Unit Test by definition")]
        public static void AssertIsUnitTest(string exceptionMessageFormat, params object[] args)
        {
            if (!IsUnitTest)
            {
                string message = string.Format(exceptionMessageFormat, args) + "(execution path=" + _executionPath + ")";
                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Throws a NotSupportedException if we are not in Debug
        /// </summary>
        /// <remarks>By having this in the library, we don't have to mark the consumer method as CodeCoverageExcluded for unit tests
        /// Do not simplify to calling "IsDebug" to test as otherwise the calling assembly is the utility assembly
        /// </remarks>
        [CodeCoverageExcluded("Cannot cover this in a Unit Test by definition")]
        public static void AssertIsDebug(string exceptionMessageFormat, params object[] args)
        {
            Assembly callingAssmebly = Assembly.GetCallingAssembly();
            EnsureCustomAttributeExists(callingAssmebly);

            if (!_compilationAttribute.IsDebug)
            {
                throw new NotSupportedException(string.Format(exceptionMessageFormat, args) + " (calling assembly=" + callingAssmebly.FullName + ")");
            }
        }

        /// <summary>
        /// Find the custom attribute in the assembly that made the call to IsDebug
        /// </summary>
        [CodeCoverageExcluded("Cannot cover this in a Unit Test since it depends on compile attributes")]
        private static T GetCustomAttribute<T>(Assembly assembly) where T : Attribute
        {
            object[] customAttributes = assembly.GetCustomAttributes(false);
            T customAttribute = null;
            foreach (Attribute attribute in customAttributes)
            {
                customAttribute = attribute as T;

                if (customAttribute != null)
                {
                    break;
                }
            }
            return customAttribute;
        }

        private static void EnsureCustomAttributeExists(Assembly assembly)
        {
            // If already set, then no work to do
            if (_compilationAttribute != null)
            {
                return;
            }

            _compilationAttribute = GetCustomAttribute<AssemblyCompilationAttribute>(assembly);

            // If we can't find it then the call to IsDebugBits is illegal
            if (_compilationAttribute == null)
            {
                throw new NotSupportedException("You must mark your [" + assembly.FullName + "] assembly with the AssemblyCompilationAttribute by adding to AssemblyInfo.cs");
            }
        }
        #endregion
    }
}
