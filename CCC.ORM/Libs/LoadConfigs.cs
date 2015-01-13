using System.Configuration;

namespace CCC.ORM.Libs
{
    public class LoadConfigs
    {
        public LoadConfigs()
        {
            DllConfig = ConfigurationManager.OpenExeConfiguration(GetType().Assembly.Location);
        }

        public Configuration DllConfig { get; private set; }

        public AppSettingsSection LoadConfigsSection(string sectionName)
        {
            return (AppSettingsSection) DllConfig.GetSection(sectionName);
        }

        public string GetConfigValue(string sectionName, string key)
        {
            return LoadConfigsSection(sectionName).Settings[key].Value;
        }
    }
}