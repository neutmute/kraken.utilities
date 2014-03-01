using System.Reflection;

[assembly: AssemblyTitle("Kraken.NLog")]
[assembly: AssemblyDescription("NLog extensions")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.NLog (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.NLog (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

