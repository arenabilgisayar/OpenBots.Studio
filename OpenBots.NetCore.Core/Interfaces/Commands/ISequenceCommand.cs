using OpenBots.NetCore.Core.Command;
using System.Collections.Generic;

namespace OpenBots.NetCore.Core.Infrastructure
{
    public interface ISequenceCommand
    {
        List<ScriptCommand> ScriptActions { get; set; }
        string v_Comment { get; set; }
        string GetDisplayValue();
    }
}
