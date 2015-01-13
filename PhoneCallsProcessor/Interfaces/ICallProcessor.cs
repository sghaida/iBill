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