using System.Reflection;

[assembly: AssemblyTitle("Sample.Tests.Xunit")]
[assembly: AssemblyDescription("Demonstrate use with XUnit")]

#if DEBUG
[assembly: AssemblyProduct("Sample.Tests.Xunit (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Sample.Tests.Xunit (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

