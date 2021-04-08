namespace OpenBots.Commands.Documents.Interfaces
{
    public interface IRequest
    {
        string v_TenantId { get; set; } //long?     
        string v_Username { get; set; }
        string v_Password { get; set; }
        string v_ApiKey { get; set; }
    }
}
