using OpenBots.NetCore.Core.Command;
using OpenBots.NetCore.Core.Enums;
using OpenBots.NetCore.Core.Model.EngineModel;
using OpenBots.NetCore.Core.UI.Controls;
using System.Collections.Generic;

namespace OpenBots.NetCore.Core.Infrastructure
{
    public interface IfrmCommandEditor
    {
        List<AutomationCommand> CommandList { get; set; }
        EngineContext ScriptEngineContext { get; set; }
        ScriptCommand SelectedCommand { get; set; }
        ScriptCommand OriginalCommand { get; set; }
        CreationMode CreationModeInstance { get; set; }
        string DefaultStartupCommand { get; set; }
        ScriptCommand EditingCommand { get; set; }
        List<ScriptCommand> ConfiguredCommands { get; set; }
        string HTMLElementRecorderURL { get; set; }
        TypeContext TypeContext { get; set; }
    }
}
