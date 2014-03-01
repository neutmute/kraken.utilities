using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Autofac")]
[assembly: AssemblyDescription("Utility belt for registering the container")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Autofac (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Kraken.Autofac (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
[assembly: AssemblyConfiguration("Release")]
#endif

