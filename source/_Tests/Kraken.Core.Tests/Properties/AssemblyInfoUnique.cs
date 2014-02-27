using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Framework.Core")]
[assembly: AssemblyDescription("Utilities")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Framework.Core.Tests (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly: AssemblyProduct("Kraken.Framework.Core.Tests (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

