using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TextXtractor.Ocr.Core
{
    public class BoundingBox 
    {

        public const string BOXTYPE_PAGE = "Page";
        public const string BOXTYPE_LINE = "Line";
        public const string BOXTYPE_BLOCK = "Block";
        public const string BOXTYPE_REGION = "Region";
        public const string BOXTYPE_WORD = "Word";
        public const string BOXTYPE_SYMBOL = "Symbol";

        public BoundingBox()
        {
            Boxes = new List<BoundingBox>();
        }

        public BoundingBox(Rectangle rectangle) : this()
        {
            TopLeft = new Point(rectangle.X, rectangle.Y);
            BottomLeft = new Point(rectangle.X, rectangle.Bottom);
            TopRight = new Point(rectangle.Right, rectangle.Y);
            BottomRight = new Point(rectangle.Right, rectangle.Bottom);
        }


        public BoundingBox(Point topLeft, Point bottomLeft, Point topRight, Point bottomRight) : this()
        {
            TopLeft = topLeft;
            BottomLeft = bottomLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
        }

        public BoundingBox(Point[] points) : this()
        {
            if(points.Length >= 1)
                TopLeft = points[0];
            if (points.Length >= 2)
                TopRight = points[1];
            if (points.Length >= 3)
                BottomRight = points[2];
            if (points.Length >= 4)
                BottomLeft = points[3];
        }

        public BoundingBox(Point topLeft, Point bottomRight) : this()
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
      
            BottomLeft = new Point(topLeft.X, BottomRight.Y);
            TopRight = new Point(BottomRight.X, topLeft.Y);
        }

        public BoundingBox(Point topLeft, Size size) : this()
        {
            TopLeft = topLeft;
            BottomLeft = new Point(topLeft.X, topLeft.Y + size.Height);
            TopRight = new Point(topLeft.X + size.Width, topLeft.Y);
            BottomRight = new Point(topLeft.X + size.Width, topLeft.Y + size.Height);
        }


        public BoundingBox(int x, int y, int width, int height) : this()
        {
            TopLeft = new Point(x,y);
            Size size = new Size(width, height);
            BottomLeft = new Point(TopLeft.X, TopLeft.Y + size.Height);
            TopRight = new Point(TopLeft.X + size.Width, TopLeft.Y);
            BottomRight = new Point(TopLeft.X + size.Width, TopLeft.Y + size.Height);

        }

        public BoundingBox(double x, double y, double width, double height) : this((int)x, (int)y, (int)width, (int)height)
        {

        }

        public BoundingBox(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4) : this()
        {
            TopLeft = new Point(x1,y1);
            TopRight = new Point(x2, y2);
            BottomRight = new Point(x3, y3);
            BottomLeft = new Point(x4,y4);
            Size size = new Size(Width, Height);

        }


        public BoundingBox(double[] points) : this()
        {
            if (points.Count() == 4)
            {
                TopLeft = new Point((int)points[0], (int)points[1]);
                Size size = new Size((int)points[2], (int)points[3]);
                BottomLeft = new Point(TopLeft.X, TopLeft.Y + size.Height);
                TopRight = new Point(TopLeft.X + size.Width, TopLeft.Y);
                BottomRight = new Point(TopLeft.X + size.Width, TopLeft.Y + size.Height);
            }

            if (points.Count() == 8)
            {
                TopLeft = new Point((int)points[0], (int)points[1]);
                TopRight = new Point((int)points[2], (int)points[3]);
                BottomRight = new Point((int)points[4], (int)points[5]);
                BottomLeft = new Point((int)points[6], (int)points[7]);
                Size size = new Size(Width, Height);
            }

        }

        public BoundingBox(IList<double> points) : this(points.ToArray())
        {
            
        }


        public bool IsInside(Rectangle rectange)
        {
            return (Top >= rectange.Top && Top <= (rectange.Top + rectange.Height) && Left >= rectange.Left && Left <= (rectange.Left + rectange.Width));
        }

        public bool IsInside(BoundingBox box)
        {
            return IsInside(box.ToRectange());
        }

        public bool IsInside(Point topLeft, Size size)
        {
            return IsInside(new Rectangle(topLeft, size));
        }


        public Size ToSize()
        {
            return new Size(Width, Height);
        }

        public int Top
        {
            get
            {
                return TopLeft.Y;
            }
        }

        public int Left
        {
            get
            {
                return TopLeft.X;
            }
        }


        public int Height
        {
            get
            {
                return BottomLeft.Y - TopLeft.Y;
            }
        }

        public int Width
        {
            get
            {
                return TopRight.X - TopLeft.X;
            }
        }

        public RectangleF ToRectangeF()
        {
            return new RectangleF(TopRight, ToSize());
        }

        public Rectangle ToRectange()
        {
            return new Rectangle(TopLeft, ToSize());
        }


        public string Type { get; set; }
        public string Text { get; set; }
        
        public float Confidence { get; set; }

        public int TextPosition { get; set; }

        public int Ordinal { get; set; }

        public Point TopLeft { get; set; }

        public Point BottomLeft { get; set; }

        public Point TopRight { get; set; }

        public Point BottomRight { get; set; }


        public List<BoundingBox> Boxes { get; set; }

    }
}
