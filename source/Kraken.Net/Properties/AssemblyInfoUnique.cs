using System.Reflection;

[assembly: AssemblyTitle("Kraken.Net")]
[assembly: AssemblyDescription("Network related utilities")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Net (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.Net (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

