using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;
using NLog;

namespace Kraken.Core.ExtensionMethods
{
    public static class ContainerBuilderExtensions
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void RegisterAllModules(this ContainerBuilder containerBuilder)
        {
            RegisterAllModules(containerBuilder, "*.dll");
        }

        public static void RegisterTypes(this ContainerBuilder builder, Type assemblyContainingType, params string[] endsWith)
        {
            var assembly = assemblyContainingType.Assembly;
            RegisterTypes(builder, assembly, endsWith);
        }

        public static void RegisterTypes(this ContainerBuilder builder, Assembly assembly, params string[] endsWith)
        {
            var endsWithList = new List<string>();
            endsWithList.AddRange(endsWith);

            Log.Trace("Registering services ending with '{1}' for {0}", assembly.GetName().Name, endsWithList.ToCsv("', '"));
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => endsWithList.Exists(s => t.Name.EndsWith(s)))
             .AsImplementedInterfaces()
             .AsSelf();
        }


        public static void RegisterAllOfType<T>(this ContainerBuilder builder)
        {
            ReflectionService reflectionService = new ReflectionService();
            List<Assembly> assemblies = reflectionService.GetLocalAssemblies();
            List<Type> allTypesFromAllAssemblies = reflectionService.GetAllTypes(assemblies);
            List<Type> typeImplementations = reflectionService.GetImplementationsOf<T>(allTypesFromAllAssemblies);

            Log.Trace("Found {0} types implementing {2}: {1}", typeImplementations.Count, typeImplementations.ToCsv(", ", t => t.Name), typeof(T).Name);

            typeImplementations.ForEach(t =>
            {
                Log.Trace("Registering {0}", t.Name);
                builder.RegisterType(t).PropertiesAutowired();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RegisterAllModules(this ContainerBuilder containerBuilder, params string[] searchPatterns)
        {
            ReflectionService reflectionService = new ReflectionService();

            Log.Trace("Searching for and registering all Autofac Modules");
            List<Assembly> assemblies = reflectionService.GetLocalAssemblies(searchPatterns);

            Log.Trace(
                "Found {0} assemblies: {1}"
                , assemblies.Count
                , assemblies.ToCsv(", ", a => a.GetName().Name));

            List<Type> allTypesFromAllAssemblies = reflectionService.GetAllTypes(assemblies);

            List<Type> candidateAutofacAssemblyTypes = (
                                                from t in allTypesFromAllAssemblies
                                                .Where(type => typeof(Autofac.Module).IsAssignableFrom(type))
                                                .Where(type => type.GetConstructor(Type.EmptyTypes) != null)
                                                select t
                                            ).ToList();

            Log.Trace("Found {0} candidate AutofacModule types: {1}", candidateAutofacAssemblyTypes.Count, candidateAutofacAssemblyTypes.ToCsv(", ", t => t.FullName));

            candidateAutofacAssemblyTypes
                .Select(Activator.CreateInstance)
                .Cast<IModule>()
                .ToList()
                .ForEach(module =>
                {
                    Log.Trace("Registering Autofac Module: '{0}'", module.GetType().FullName);
                    containerBuilder.RegisterModule(module);
                });
        }
    }
}
