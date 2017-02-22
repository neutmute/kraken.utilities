using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Core.Tests")]
[assembly: AssemblyDescription("Utilities")]

#if DEBUG
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

