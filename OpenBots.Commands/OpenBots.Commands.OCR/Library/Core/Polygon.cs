using Loyc.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TextXtractor.Ocr.Core
{
    public static class Polygon
    {
        public static List<Point> MergePolygon(Point[] points)
        {
            var pts = points
              .Select(p => new Point<int>(p.X, p.Y)).ToList();

            return PointMath.ComputeConvexHull(pts, false)
              .Select(p => new Point(p.X, p.Y)).ToList();
        }

        public static List<PointF> MergePolygon(List<PointF> eqPoints, List<PointF> paragraphPoints)
        {
            var pts = eqPoints.Union(paragraphPoints)
              .Select(p => new Point<float>(p.X, p.Y)).ToList();
            return PointMath.ComputeConvexHull(pts, false)
              .Select(p => new PointF(p.X, p.Y)).ToList();
        }

        public static List<PointF> Sort(List<PointF> eqPoints)
        {
            var pts = eqPoints.Select(p => new Point<float>(p.X, p.Y)).ToList();
            return PointMath.ComputeConvexHull(pts, true)
              .Select(p => new PointF(p.X, p.Y)).ToList();
        }
        public static bool IsInPolygon(this Point point, IEnumerable<Point> polygon)
        {
            bool result = false;
            var a = polygon.Last();
            foreach (var b in polygon)
            {
                if ((b.X == point.X) && (b.Y == point.Y))
                    return true;

                if ((b.Y == a.Y) && (point.Y == a.Y) && (a.X <= point.X) && (point.X <= b.X))
                    return true;

                if ((b.Y < point.Y) && (a.Y >= point.Y) || (a.Y < point.Y) && (b.Y >= point.Y))
                {
                    if (b.X + (point.Y - b.Y) / (a.Y - b.Y) * (a.X - b.X) <= point.X)
                        result = !result;
                }
                a = b;
            }
            return result;
        }

        private static void CalculateOuterBounds(PointF[] mAptVertices)
        {
            //m_aptVertices is a Point[] which holds the vertices of the polygon.
            // and X/Y min/max are just ints
            var Xmin = mAptVertices[0].X;
            var Xmax = mAptVertices[0].X;
            var Ymin = mAptVertices[0].Y;
            var Ymax = mAptVertices[0].Y;

            foreach (PointF pt in mAptVertices)
            {
                if (Xmin > pt.X)
                    Xmin = pt.X;

                if (Xmax < pt.X)
                    Xmax = pt.X;

                if (Ymin > pt.Y)
                    Ymin = pt.Y;

                if (Ymax < pt.Y)
                    Ymax = pt.Y;
            }
        }

        // Find the slope of the edge from point i to point i+1.
        private static void FindDxDy(PointF[] Points, int NumPoints, out float dx, out float dy, int i)
        {
            int i2 = (i + 1) % NumPoints;
            dx = Points[i2].X - Points[i].X;
            dy = Points[i2].Y - Points[i].Y;
        }

        // Find the point of intersection between two lines.
        private static bool FindIntersection(float X1, float Y1, float X2, float Y2, float A1, float B1, float A2, float B2, ref PointF intersection)
        {
            float dx = X2 - X1;
            float dy = Y2 - Y1;
            float da = A2 - A1;
            float db = B2 - B1;
            float s, t;

            // If the segments are parallel, return False.
            if (Math.Abs(da * dy - db * dx) < 0.001) return false;

            // Find the point of intersection.
            s = (dx * (B1 - Y1) + dy * (X1 - A1)) / (da * dy - db * dx);
            t = (da * (Y1 - B1) + db * (A1 - X1)) / (db * dx - da * dy);
            intersection = new PointF(X1 + t * dx, Y1 + t * dy);
            return true;
        }
    }
}
