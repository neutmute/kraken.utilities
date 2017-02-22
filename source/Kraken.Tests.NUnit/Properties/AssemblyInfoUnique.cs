using System.Reflection;

[assembly: AssemblyTitle("Kraken.Tests.NUnit")]
[assembly: AssemblyDescription("Nunit base fixtures")]

#if DEBUG
[assembly: AssemblyProduct("Kraken.Tests.NUnit(Debug)")]
#else
[assembly: AssemblyProduct("Kraken.Tests.NUnit (Release)")]
#endif

