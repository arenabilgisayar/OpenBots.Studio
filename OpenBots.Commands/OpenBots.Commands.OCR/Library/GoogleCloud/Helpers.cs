using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TextXtractor.Ocr.GoogleCloud
{
    internal static class Helpers
    {
        public static Point[] ToPoints(this Vertex[] points)
        {
            return points.Select(p => new Point(p.X, p.Y)).ToArray();
        }
    }
}
