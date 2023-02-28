namespace CommandService.EventProcessing.Interfaces
{
    public interface IEventProcessor
    {
        void Process(string message);
    }
}