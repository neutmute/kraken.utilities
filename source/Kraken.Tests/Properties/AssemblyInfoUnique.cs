using System.Reflection;

[assembly: AssemblyTitle("Kraken.Tests")]
[assembly: AssemblyDescription("Unit Test utilities")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Tests (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.Tests (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

