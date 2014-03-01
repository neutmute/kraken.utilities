using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Core")]
[assembly: AssemblyDescription("Collection of utilities")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Core (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.Core (Release)")]
[assembly: AssemblyCompilation(BuildConfiguration.Release)]
[assembly: AssemblyConfiguration("Release")]
#endif

