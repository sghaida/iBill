using PhoneCallsProcessorLoader.ConfigurationSections;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PhoneCallsProcessor.Interfaces;

namespace PhoneCallsProcessorLoader
{
    public class PluginLoader<T>
    {
        public static List<ICallProcessor> LoadPlugins() 
        {
            //Read Plugins Configurations
            CallProcessorPluginsSection cppSection = (CallProcessorPluginsSection)ConfigurationManager.GetSection(CallProcessorPluginsSection.ConfigurationSectionName);
            List<CallProcessorPluginData> pluginsData = cppSection.CallProcessorPluginsList;

            List<Assembly> assemblies = new List<Assembly>();

            //Put assemblies in a list
            foreach (CallProcessorPluginData pluginInfo in pluginsData) 
            {
                if (pluginInfo.Enabled == true) 
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(pluginInfo.Path);
                    Assembly assembly = Assembly.Load(an);
                    assemblies.Add(assembly); 
                }
            }

            Type pluginType = typeof(ICallProcessor);

            List<Type> pluginTypes = new List<Type>();

            foreach (Assembly asm in assemblies) 
            {
                if (asm != null) 
                {
                    Type[] types = asm.GetTypes();

                    foreach (Type type in types) 
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
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

            List<ICallProcessor> plugins = new List<ICallProcessor>(pluginTypes.Count);

            foreach (Type type in pluginTypes)
            {
                ICallProcessor plugin = (ICallProcessor)Activator.CreateInstance(type);
                plugins.Add(plugin);
            }

            return plugins; 

        }
        
    }
}
