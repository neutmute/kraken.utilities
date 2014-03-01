using System;
using System.Runtime.InteropServices;

namespace Kraken.Core
{
    public enum BuildConfiguration
    {
        [EnumDisplayName("Unknown Build Configuration")]
        Unknown = 0,

        /// <summary>
        /// Denote this as a debug compiled assembly
        /// </summary>
        Debug,

        /// <summary>
        /// Denote this as a release compiled assembly
        /// </summary>
        Release
    }

    /// <summary>
    /// This attribute is used to mark an assembly at compile time as compiled in either debug or release.
    /// The ExecutionEnvironment.IsDebug parameter can then be called which will test the calling assemblies
    /// attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false), ComVisible(true)]
    public sealed class AssemblyCompilationAttribute : Attribute
    {
        public BuildConfiguration BuildConfiguration { get; private set; }

        #region Constructors
        public AssemblyCompilationAttribute(BuildConfiguration buildConfiguration)
        {
            BuildConfiguration = buildConfiguration;
        }
        #endregion

        #region Properties
        public bool IsDebug 
        { 
            get
            {
                return BuildConfiguration == BuildConfiguration.Debug;
            }
        }
        #endregion
    }
}


