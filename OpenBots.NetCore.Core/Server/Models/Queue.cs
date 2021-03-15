namespace OpenBots.NetCore.Core.Server.Models
{
    public class Queue : NamedEntity
    {
        public string Description { get; set; }
        public int MaxRetryCount { get; set; }
    }
}
