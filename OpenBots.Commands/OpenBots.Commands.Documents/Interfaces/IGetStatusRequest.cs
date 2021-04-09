namespace OpenBots.Commands.Documents.Interfaces
{
    public interface IGetStatusRequest : IRequest
    {
        string v_TaskId { get; set; } //Guid
        string v_AwaitCompletion { get; set; } //bool
        string v_Timeout { get; set; } //int
    }
}