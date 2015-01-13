using System;
using PhoneCallsProcessor.Interfaces;

namespace PhoneCallsProcessorLoader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var plugins = PluginLoader<ICallProcessor>.LoadPlugins();

            plugins[0].ProcessPhoneCalls();

            Console.ReadLine();
        }
    }
}