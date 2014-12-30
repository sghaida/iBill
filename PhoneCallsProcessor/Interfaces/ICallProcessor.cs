using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneCallsProcessor.Interfaces
{
    public interface ICallProcessor
    {
        string Name { get; }
        string Description { get; }
        string Version { get; }

        void PluginInfo();

        void ProcessPhoneCalls();
    }
}
