using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Noda")]
[assembly: AssemblyDescription("Noda time extensions")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Noda (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.Noda (Release)")]
[assembly: AssemblyCompilation(BuildConfiguration.Release)]
[assembly: AssemblyConfiguration("Release")]
#endif

