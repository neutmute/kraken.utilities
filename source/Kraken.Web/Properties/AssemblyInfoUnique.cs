using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Web")]
[assembly: AssemblyDescription("Web, WCF related extensions and helpers")]
#if DEBUG
[assembly: AssemblyProduct("Kraken.Web (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly: AssemblyProduct("Kraken.Web (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

