using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public class BarcodeBox : BoundingBox
    {
        public BarcodeBox()
        {
        }

        public BarcodeBox(Rectangle rectangle) : base(rectangle)
        {
        }

        public string BarcodeType { get; set; }
        public string BarcodeKind { get; set; }
       
    }
}
