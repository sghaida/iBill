using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PhoneCallsProcessorLoader.ConfigurationSections;
using PhoneCallsProcessor.Interfaces;



namespace PhoneCallsProcessorLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ICallProcessor> plugins = PluginLoader<ICallProcessor>.LoadPlugins();

            plugins[0].ProcessPhoneCalls();

            Console.ReadLine();
        
        }
    }
}
