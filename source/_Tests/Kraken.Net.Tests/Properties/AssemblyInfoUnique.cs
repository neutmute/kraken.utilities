using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Net.Tests")]
[assembly: AssemblyDescription("Utilities")]


#if DEBUG
[assembly: AssemblyProduct("Kraken.Core.Tests (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.Core.Tests (Release)")]
[assembly: AssemblyCompilation(BuildConfiguration.Release)]
[assembly: AssemblyConfiguration("Release")]
#endif

