using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Core.Tests")]
[assembly: AssemblyDescription("Utilities")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Tests.Tests (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly: AssemblyProduct("Kraken.Tests.Tests (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

