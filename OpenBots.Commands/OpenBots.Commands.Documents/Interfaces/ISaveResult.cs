namespace OpenBots.Commands.Documents.Interfaces
{
    public interface ISaveResult
    {
        string v_HasFailedOrError { get; set; } //bool
        string v_IsCompleted { get; set; } //bool
        string v_Status { get; set; }
    }
}