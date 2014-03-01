using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Web.Tests")]
[assembly: AssemblyDescription("Utilities")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Web.Tests (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly: AssemblyProduct("Kraken.Web.Tests (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

