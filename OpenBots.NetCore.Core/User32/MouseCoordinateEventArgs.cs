using OpenBots.NetCore.Core.Enums;
using System;
using System.Windows;

namespace OpenBots.NetCore.Core.User32
{
    public class MouseCoordinateEventArgs : EventArgs
    {
        public Point MouseCoordinates { get; set; }
        public MouseMessages MouseMessage { get; set; }
    }
}
