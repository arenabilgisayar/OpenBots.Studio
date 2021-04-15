using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TextXtractor.Ocr.Core
{
    public static class Helpers
    {
        public static bool CornerPointAlmostEqual(PointF[] rect1, PointF[] rect2, double eps)
        {
            var almostEqual = true;
            for (int i = 0; i < 2; i++)
                if (!(Math.Abs(rect1[i].Y - rect2[i].Y) < eps))
                    almostEqual = false;
            return almostEqual;
        }

        public static bool IsPointInPolygon4(this PointF testPoint, PointF[] polygon)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
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

        public static IEnumerable<string> Iterate(this int[] limits, Func<int, string> action)
        {
            for (var i = limits[0]; i < limits[1]; i++)
                yield return action(i);
        }

        public static IEnumerable<T1> ForEach<T, T1>(this IEnumerable<T> source, Func<T, T1> action)
        {
            foreach (var s in source)
                yield return action(s);
        }

        public static string Join(this IEnumerable<string> source, string separator = "")
        {
            return string.Join(separator, source);
        }

        public static string EmptyIfNull(this string source)
        {
            return !string.IsNullOrWhiteSpace(source) ? source : "";
        }

        public static string GetIndexOrEmpty(this string[] source, int index)
        {
            return source.Length > index ? source[index] : "";
        }

        public static bool IsIn(this List<int> source, List<int> dest)
        {
            if (source.Count > dest.Count)
                return false;

            if (source.Count == dest.Count)
                return !source.Except(dest).Any();

            foreach (var s in source)
            {
                if (!dest.Contains(s))
                    return false;
            }
            return true;
        }
    }
}
