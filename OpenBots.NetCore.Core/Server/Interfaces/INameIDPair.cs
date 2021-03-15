using System;

namespace OpenBots.NetCore.Core.Server.Interfaces
{
    public interface INameIDPair
    {
        Guid? Id { get; set; }
        string Name { get; set; }
    }
}
