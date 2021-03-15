using System;

namespace OpenBots.NetCore.Core.Server.Models
{
    public class QueueItemAttachment : Entity
    {
        public Guid QueueItemId { get; set; }
        public Guid FileId { get; set; }
    }
}
