using System;

namespace OpenBots.NetCore.Core.Model.EngineModel
{
    public class LineNumberChangedEventArgs : EventArgs
    {
        public int CurrentLineNumber { get; set; }
    }
}
