using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Logging;

namespace Kraken.Core
{
    public class ReflectionService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();


        public List<Assembly> GetLocalAssemblies()
        {
            return GetLocalAssemblies("*.dll", "*.exe");
        }

        public List<Assembly> GetLocalAssemblies(params string[] searchPatterns)
        {
            string applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            List<string> files = new List<string>();
            foreach(string searchPattern in searchPatterns)
            {
                var thisDirectory = Directory.EnumerateFiles(applicationDirectory, searchPattern, SearchOption.TopDirectoryOnly).ToList();
                
                // dirty hack to stop nsis uninstaller being loaded as it isn't an assembly
                thisDirectory.RemoveAll(f => f.ToLower().EndsWith("uninstall.exe"));

                files.AddRange(thisDirectory);
            }

            return new List<Assembly>(files.Select(Assembly.LoadFrom));
        }

        public List<Type> GetAllTypes(IEnumerable<Assembly> assemblies)
        {
            // BH: This code had some funky all in one linq which failed on Assembly.GetTypes() in a solution
            // Needed to unroll and add some try catch handling to allow service to start
            List<Type> allAssemblyTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    List<Type> thisAssemblyTypes = assembly.GetTypes().ToList();
                    allAssemblyTypes.AddRange(thisAssemblyTypes);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Exception exSub in ex.LoaderExceptions)
                    {
                        sb.AppendLine(exSub.Message);
                        if (exSub is FileNotFoundException)
                        {
                            FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                            if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                            {
                                sb.AppendLine("Fusion Log:");
                                sb.AppendLine(exFileNotFound.FusionLog);
                            }
                        }
                        sb.AppendLine();
                    }
                    string errorMessage = sb.ToString();
                    Log.Warn(errorMessage, ex);
                }
                catch (Exception e)
                {
                    Log.Warn("Failed to load types for " + assembly.GetName().Name, e);
                    continue;
                }
            }

            return allAssemblyTypes;
        }

        public List<Type> GetImplementationsOf<T>(List<Assembly> assemblies)
        {
            List<Type> allTypes = GetAllTypes(assemblies);
            return GetImplementationsOf<T>(allTypes);
        }


        public List<Type> GetImplementationsOf<T>(List<Type> types)
        {
            var stage1 = (from t in types where typeof(T).IsAssignableFrom(t) select t).ToList();
            var stage2 = (from t in stage1 where !t.IsInterface select t).ToList();
            var stage3 = (from t in stage2 where !t.IsAbstract select t).ToList();

            //List<Type> typeList = (
            //                          from t in types
            //                              .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            //                          select t
            //                      ).ToList();

            return stage3;
        }
    }
}