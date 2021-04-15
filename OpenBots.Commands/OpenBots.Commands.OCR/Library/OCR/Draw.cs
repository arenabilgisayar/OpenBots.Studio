using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr
{
    public class Draw
    {


        public void DrawBoxesAndSave(string imageFile, List<BoundingBox> boxes, string resultImageFile)
        {
            Stream stream = File.OpenRead(imageFile);
            DrawBoxesAndSave(stream, boxes, resultImageFile);
        }
        public void DrawBoxesAndSave(Stream imageStream, List<BoundingBox> boxes, string resultImageFile)
        {
            Image image = DrawBoxes(imageStream, boxes);
            image.Save(resultImageFile, ImageFormat.Png);

        }

            public Image DrawBoxes(Stream imageStream, List<BoundingBox> boxes)
        {
            Image image = Image.FromStream(imageStream);

           Graphics graphicsObj = Graphics.FromImage(image);

            Pen myPen = new Pen(System.Drawing.Color.Red, 1);

            Font myFont = new Font("Helvetica", 10, FontStyle.Regular);

            Brush myBrush = new SolidBrush(Color.Red);

            foreach (BoundingBox box in boxes)
            {
                DrawBoundingBox(box, graphicsObj, myPen, myFont, myBrush);
            }

            return image;
        }

        protected void DrawBoundingBox(BoundingBox box, Graphics graphicsObj, Pen myPen, Font myFont, Brush myBrush)
        {

            if (box.Type == BoundingBox.BOXTYPE_LINE)
            {

                Rectangle myRectangle = box.ToRectange();
                graphicsObj.DrawRectangle(myPen, myRectangle);
                graphicsObj.DrawString(box.Text, myFont, myBrush, box.Left, box.Top);
            }

            foreach (BoundingBox innerBox in box.Boxes)
            {
                DrawBoundingBox(innerBox, graphicsObj, myPen, myFont, myBrush);
            }
        }
    }
}
