using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Web")]
[assembly: AssemblyDescription("Web, WCF related extensions and helpers")]
#if DEBUG
[assembly: AssemblyProduct("Kraken.Web (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.Framework.Core (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
[assembly: AssemblyConfiguration("Release")]
#endif

