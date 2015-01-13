using System;

namespace PhoneCallsProcessorLoader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var plugins = PluginLoader.LoadPlugins();

            plugins[0].ProcessPhoneCalls();

            Console.ReadLine();
        }
    }
}