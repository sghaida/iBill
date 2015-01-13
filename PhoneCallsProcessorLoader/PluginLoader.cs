using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using PhoneCallsProcessor.Interfaces;
using PhoneCallsProcessorLoader.ConfigurationSections;

namespace PhoneCallsProcessorLoader
{
    public class PluginLoader
    {
        public static List<ICallProcessor> LoadPlugins()
        {
            //Read Plugins Configurations
            var cppSection =
                (CallProcessorPluginsSection)
                    ConfigurationManager.GetSection(CallProcessorPluginsSection.ConfigurationSectionName);
            var pluginsData = cppSection.CallProcessorPluginsList;

            var assemblies = new List<Assembly>();

            //Put assemblies in a list
            foreach (var pluginInfo in pluginsData)
            {
                if (pluginInfo.Enabled)
                {
                    var an = AssemblyName.GetAssemblyName(pluginInfo.Path);
                    var assembly = Assembly.Load(an);
                    assemblies.Add(assembly);
                }
            }

            var pluginType = typeof (ICallProcessor);

            var pluginTypes = new List<Type>();

            foreach (var asm in assemblies)
            {
                if (asm != null)
                {
                    var types = asm.GetTypes();

                    foreach (var type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.Name) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            var plugins = new List<ICallProcessor>(pluginTypes.Count);

            foreach (var type in pluginTypes)
            {
                var plugin = (ICallProcessor) Activator.CreateInstance(type);
                plugins.Add(plugin);
            }

            return plugins;
        }
    }
}