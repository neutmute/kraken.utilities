using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("Kraken.Web.Tests")]
[assembly: AssemblyDescription("Utilities")]

#if DEBUG
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

