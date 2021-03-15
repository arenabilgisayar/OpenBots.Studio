using OpenBots.NetCore.Core.Enums;
using System;

namespace OpenBots.NetCore.Core.Model.EngineModel
{
    public class ScriptFinishedEventArgs : EventArgs
    {
        public DateTime LoggedOn { get; set; }
        public ScriptFinishedResult Result { get; set; }
        public string Error { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public string FileName { get; set; }
    }
}
