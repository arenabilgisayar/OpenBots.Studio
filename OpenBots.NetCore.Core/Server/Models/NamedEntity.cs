using OpenBots.NetCore.Core.Server.Interfaces;

namespace OpenBots.NetCore.Core.Server.Models
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        public NamedEntity() : base()
        {

        }

        public string Name { get; set; }
    }
}
