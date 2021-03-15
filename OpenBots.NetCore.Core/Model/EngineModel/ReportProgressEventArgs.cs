﻿using System;
using System.Drawing;

namespace OpenBots.NetCore.Core.Model.EngineModel
{
    public class ReportProgressEventArgs : EventArgs
    {
        public string ProgressUpdate { get; set; }
        public Color LoggerColor { get; set; }
    }
}
