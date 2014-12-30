using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneCallsProcessorLoader.ConfigurationSections
{
    public class CallProcessorPluginsElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["Name"].ToString(); }
        }

        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get { return this["Path"].ToString(); }
        }

        [ConfigurationProperty("Version", IsRequired = true)]
        public string Version
        {
            get { return this["Version"].ToString(); }
        }

        [ConfigurationProperty("Enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return Convert.ToBoolean(this["Enabled"]); }
        }
    }

    public class CallProcessorPluginsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CallProcessorPluginsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CallProcessorPluginsElement)element).Name;
        }
    }

    public class CallProcessorPluginsSection : ConfigurationSection
    {
        public static string ConfigurationSectionName
        {
            get { return "CallProcessorPluginsSection"; }
        }

        [ConfigurationProperty("CallProcessorPlugins")]
        public CallProcessorPluginsCollection CallProcessorPlugins
        {
            get { return (CallProcessorPluginsCollection)this["CallProcessorPlugins"]; }
        }


        public List<CallProcessorPluginData> CallProcessorPluginsList
        {
            get
            {
                List<CallProcessorPluginData> CallProcessorPluginsList = new List<CallProcessorPluginData>();

                foreach (CallProcessorPluginsElement el in CallProcessorPlugins)
                {
                    CallProcessorPluginData pluginData = new CallProcessorPluginData();
                    
                    pluginData.Name = el.Name;
                    pluginData.Path = el.Path;
                    pluginData.Version = el.Version;
                    pluginData.Enabled = el.Enabled;

                    CallProcessorPluginsList.Add(pluginData);
                }

                return CallProcessorPluginsList;
            }
        }

    }
}
