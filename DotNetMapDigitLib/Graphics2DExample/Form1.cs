using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Mapdigit.Drawing;
using Mapdigit.Drawing.Geometry;
using Color = Mapdigit.Drawing.Color;
using Path = Mapdigit.Drawing.Geometry.Path;
using Pen = Mapdigit.Drawing.Pen;
using Point = Mapdigit.Drawing.Geometry.Point;
using Rectangle = Mapdigit.Drawing.Geometry.Rectangle;
using SolidBrush = Mapdigit.Drawing.SolidBrush;

namespace Graphics2DExample
{
    public partial class Form1 : Form
    {
        private readonly Graphics2D _graphics2D;

        private readonly int _screenWidth;

        private readonly int _screenHeight;
        public Form1()
        {
            InitializeComponent();
            _screenWidth = Width;
            _screenHeight = Height;
            _graphics2D = new Graphics2D(_screenWidth, _screenHeight);
            _graphics2D.Clear(Color.White);
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /**
   * The solid (full opaque) red color in the ARGB space
   */
            Color redColor = new Color(0xffff0000, false);

            /**
             * The semi-opaque green color in the ARGB space (alpha is 0x78)
             */
            Color greenColor = new Color(0x7800ff00, true);

            /**
             * The semi-opaque blue color in the ARGB space (alpha is 0x78)
             */
            Color blueColor = new Color(0x780000ff, true);
            /**
             * The semi-opaque yellow color in the ARGB space ( alpha is 0x78)
             */
            Color yellowColor = new Color(0x78ffff00, true);

            /**
             * The dash array
             */
            int[] dashArray = { 20, 8 };
            _graphics2D.Reset();
            _graphics2D.Clear(Color.Black);
            SolidBrush brush = new SolidBrush(redColor);
            _graphics2D.FillOval(brush, 30, 60, 80, 80);
            brush = new SolidBrush(greenColor);
            _graphics2D.FillOval(brush, 60, 30, 80, 80);
            Pen pen = new Pen(yellowColor, 10, Pen.CapButt, Pen.JoinMiter, dashArray, 0);
            brush = new SolidBrush(blueColor);
            _graphics2D.SetPenAndBrush(pen, brush);
            _graphics2D.FillOval(null, 90, 60, 80, 80);
            _graphics2D.DrawOval(null, 90, 60, 80, 80);
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawRGB(e.Graphics, _graphics2D.Argb, 0, _screenWidth, 0, 0, _screenWidth, _screenHeight, true);
        }

        public void DrawRGB(Graphics graphics, int[] rgbData, int offset, int scanlength, int x, int y, int w, int h, bool processAlpha)
        {
            Bitmap bmp = new Bitmap(w, h);
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);

            System.Drawing.Imaging.BitmapData bmpData =
           bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb
           );

            IntPtr ptr = bmpData.Scan0;

            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, ptr, rgbData.Length);

            bmp.UnlockBits(bmpData);



            //for (int i = 0; i < w; i++)
            //{
            //    for (int j = 0; j < h; j++)
            //    {
            //        image.SetPixel(i, j, System.Drawing.Color.FromArgb(rgbData[offset + (i - x) + (j - y) * scanlength]));
            //    }
            //}
            graphics.DrawImage(bmp, x, y);

        }

        private void lineCapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color blackColor = new Color(0x000000);
            Color whiteColor = new Color(0xffffff);
            _graphics2D.Reset();
            _graphics2D.Clear(Color.White);

            Pen pen = new Pen(blackColor, 20, Pen.CapButt, Pen.JoinMiter);
            _graphics2D.DrawLine(pen, 40, 60, 140, 60);
            pen = new Pen(whiteColor, 1);
            _graphics2D.DrawLine(pen, 40, 60, 140, 60);


            pen = new Pen(blackColor, 20, Pen.CapRound, Pen.JoinMiter);
            _graphics2D.DrawLine(pen, 40, 100, 140, 100);
            pen = new Pen(whiteColor, 1);
            _graphics2D.DrawLine(pen, 40, 100, 140, 100);

            pen = new Pen(blackColor, 20, Pen.CapSquare, Pen.JoinMiter);
            _graphics2D.DrawLine(pen, 40, 140, 140, 140);
            pen = new Pen(whiteColor, 1);
            _graphics2D.DrawLine(pen, 40, 140, 140, 140);
            Invalidate();
        }

        private void lineJoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color blackColor = new Color(0x000000);
            Color whiteColor = new Color(0xffffff);
            Path path = new Path();
            path.MoveTo(40, 80);
            path.LineTo(90, 40);
            path.LineTo(140, 80);
            _graphics2D.Reset();
            _graphics2D.Clear(Color.White);

            AffineTransform matrix = new AffineTransform();
            _graphics2D.AffineTransform = matrix;
            Pen pen = new Pen(blackColor, 20, Pen.CapButt, Pen.JoinMiter);
            _graphics2D.Draw(pen, path);
            pen = new Pen(whiteColor, 1);
            _graphics2D.Draw(pen, path);


            matrix.Translate(0, 50);
            _graphics2D.AffineTransform = matrix;

            pen = new Pen(blackColor, 20, Pen.CapButt, Pen.JoinRound);
            _graphics2D.Draw(pen, path);
            pen = new Pen(whiteColor, 1);
            _graphics2D.Draw(pen, path);

            matrix = new AffineTransform();
            matrix.Translate(0, 100);
            _graphics2D.AffineTransform = matrix;

            pen = new Pen(blackColor, 20, Pen.CapButt, Pen.JoinBevel);
            _graphics2D.Draw(pen, path);
            pen = new Pen(whiteColor, 1);
            _graphics2D.Draw(pen, path);
            Invalidate();
        }

        private void dashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color blackColor = new Color(0x000000);
            int[] dashArray1 = { 2, 2 };
            int[] dashArray2 = { 6, 6 };
            int[] dashArray3 = { 4, 1, 2, 1, 1, 6 };
            _graphics2D.Reset();
            _graphics2D.Clear(Color.White);

            Pen pen = new Pen(blackColor, 20, Pen.CapButt, Pen.JoinMiter, dashArray1, 0);
            _graphics2D.DrawLine(pen, 40, 60, 140, 60);

            pen = new Pen(blackColor, 20, Pen.CapButt, Pen.JoinMiter, dashArray2, 0);
            _graphics2D.DrawLine(pen, 40, 100, 140, 100);

            pen = new Pen(blackColor, 20, Pen.CapButt, Pen.JoinMiter, dashArray3, 0);
            _graphics2D.DrawLine(pen, 40, 140, 140, 140);
            Invalidate();
        }

        private void pearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ellipse circle, oval, leaf, stem;
            Area circ, ov, leaf1, leaf2, st1, st2;
            circle = new Ellipse();
            oval = new Ellipse();
            leaf = new Ellipse();
            stem = new Ellipse();
            circ = new Area(circle);
            ov = new Area(oval);
            leaf1 = new Area(leaf);
            leaf2 = new Area(leaf);
            st1 = new Area(stem);
            st2 = new Area(stem);
            _graphics2D.Reset();
            _graphics2D.Clear(Color.White);
            int w = _screenWidth;
            int h = _screenHeight;
            int ew = w / 2;
            int eh = h / 2;
            SolidBrush brush = new SolidBrush(Color.Green);
            _graphics2D.DefaultBrush = brush;
            // Creates the first leaf by filling the intersection of two Area
            //objects created from an ellipse.
            leaf.SetFrame(ew - 16, eh - 29, 15, 15);
            leaf1 = new Area(leaf);
            leaf.SetFrame(ew - 14, eh - 47, 30, 30);
            leaf2 = new Area(leaf);
            leaf1.Intersect(leaf2);
            _graphics2D.Fill(null, leaf1);

            // Creates the second leaf.
            leaf.SetFrame(ew + 1, eh - 29, 15, 15);
            leaf1 = new Area(leaf);
            leaf2.Intersect(leaf1);
            _graphics2D.Fill(null, leaf2);

            brush = new SolidBrush(Color.Black);
            _graphics2D.DefaultBrush = brush;

            // Creates the stem by filling the Area resulting from the
            //subtraction of two Area objects created from an ellipse.
            stem.SetFrame(ew, eh - 42, 40, 40);
            st1 = new Area(stem);
            stem.SetFrame(ew + 3, eh - 47, 50, 50);
            st2 = new Area(stem);
            st1.Subtract(st2);
            _graphics2D.Fill(null, st1);

            brush = new SolidBrush(Color.Yellow);
            _graphics2D.DefaultBrush = brush;

            // Creates the pear itself by filling the Area resulting from the
            //union of two Area objects created by two different ellipses.
            circle.SetFrame(ew - 25, eh, 50, 50);
            oval.SetFrame(ew - 19, eh - 20, 40, 70);
            circ = new Area(circle);
            ov = new Area(oval);
            circ.Add(ov);
            _graphics2D.Fill(null, circ);
            Invalidate();
        }

        private void ovalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color redColor = new Color(0x96ff0000, true);
            Color greenColor = new Color(0x00ff00);
            AffineTransform mat1;
            mat1 = new AffineTransform();
            mat1.Translate(30, 40);
            mat1.Rotate(-30 * Math.PI / 180.0);
            _graphics2D.Reset();
            _graphics2D.Clear(Color.White);

            _graphics2D.AffineTransform = new AffineTransform();
            SolidBrush brush = new SolidBrush(greenColor);
            _graphics2D.FillOval(brush, 20, 60, 100, 50);

            Pen pen = new Pen(redColor, 5);
            _graphics2D.AffineTransform = mat1;
            _graphics2D.DrawOval(pen, 20, 60, 100, 50);
            Invalidate();
        }

        private void pathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AffineTransform mat1;

            /* The path.  */
            Path path;

            /** Colors */
            Color redColor = new Color(0x96ff0000, true);
            Color greenColor = new Color(0xff00ff00, false);
            Color blueColor = new Color(0x750000ff, true);

            string pathdata =
                  "M 60 20 Q -40 70 60 120 Q 160 70 60 20 z";
            mat1 = new AffineTransform();
            mat1.Translate(30, 40);
            mat1.Rotate(-30 * Math.PI / 180.0);
            path = Path.FromString(pathdata);
            //Clear the canvas with white color.
            _graphics2D.Reset();
            _graphics2D.Clear(Color.White);

            _graphics2D.AffineTransform = new AffineTransform();
            SolidBrush brush = new SolidBrush(greenColor);
            _graphics2D.Fill(brush, path);
            _graphics2D.AffineTransform = mat1;

            brush = new SolidBrush(blueColor);
            Pen pen = new Pen(redColor, 5);
            _graphics2D.SetPenAndBrush(pen, brush);
            _graphics2D.Draw(null, path);
            _graphics2D.Fill(null, path);
            Invalidate();
        }

        private void polysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AffineTransform mat1;



            /** Colors */
            Color redColor = new Color(0x96ff0000, true);
            Color greenColor = new Color(0xff00ff00, false);
            Color blueColor = new Color(0x750000ff, true);

            Polyline polyline;
            Polygon polygon;
            Polygon polygon1;

            string pointsdata1 = "59,45,95,63,108,105,82,139,39,140,11,107,19,65";
            mat1 = new AffineTransform();
            mat1.Translate(30, 40);
            mat1.Rotate(-30 * Math.PI / 180.0);
            polyline = new Polyline();
            polygon = new Polygon();
            polygon1 = new Polygon();
            Point[] points = Point.FromString(pointsdata1);
            for (int i = 0; i < points.Length; i++)
            {
                polyline.AddPoint(points[i].X, points[i].Y);
                polygon.AddPoint(points[i].X, points[i].Y);
                polygon1.AddPoint(points[i].X, points[i].Y);
            }
            //Clear the canvas with white color.
            _graphics2D.Reset();
            _graphics2D.Clear(Color.White);

            _graphics2D.AffineTransform = new AffineTransform();
            SolidBrush brush = new SolidBrush(greenColor);
            _graphics2D.FillPolygon(brush, polygon);
            _graphics2D.AffineTransform = mat1;
            brush = new SolidBrush(blueColor);
            Pen pen = new Pen(redColor, 5);
            _graphics2D.SetPenAndBrush(pen, brush);
            _graphics2D.FillPolygon(null, polygon1);
            _graphics2D.DrawPolyline(null, polyline);
            Invalidate();
        }

        private void transformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Path path;

            /* The first matrix */
            AffineTransform matrix1 = new AffineTransform();
            /* The second matrix */
            AffineTransform matrix2 = new AffineTransform();
            /* The third matrix */
            AffineTransform matrix3 = new AffineTransform();

            /** Colors */
            Color blackColor = new Color(0xff000000, false);
            Color redColor = new Color(0xffff0000, false);
            Color greenColor = new Color(0xff00ff00, false);
            Color blueColor = new Color(0xff0000ff, false);
            Color fillColor = new Color(0x4f0000ff, true);

            /* Text to draw */
            char[] engText = "tiny".ToCharArray();
            FontEx font = FontEx.GetSystemFont();
            int fontSize = 16;
            int X = 20;
            int Y = 0;
            /* Define the path */
            path = new Path();
            path.MoveTo(50, 0);
            path.LineTo(0, 0);
            path.LineTo(0, 50);

            /* Define the matrix1 as  "translate(50,50)"  */
            /* Translation is the TinyMatrix [1 0 0 1 tx ty],
             * where tx and ty are the distances in x and y. */
            matrix1.Translate(50, 50);

            /* Define the matrix2 as "translate(50,50) + rotate(-45)" */
            matrix2 = new AffineTransform(matrix1);
            AffineTransform m = new AffineTransform();
            /* Rotation is TinyMatrix [cos(a) sin(a) -sin(a) cos(a) 0 0],
             * which has the effect of rotating the coordinate system
             * axes by angle a. */
            m.Rotate(-45 * Math.PI / 180.0, 0, 0);
            /* Concatenates the m  to the matrix2.
             * [matrix2] =  [matrix2] * [m];
             */
            matrix2.Concatenate(m);

            /* Define the matrix3 as
             * "translate(50,50) + rotate(-45) + translate(-20,80)"  */
            /* Copy the matrix2 to the matrix3 */
            matrix3 = new AffineTransform(matrix2);
            m = new AffineTransform();
            /* Translation is the TinyMatrix [1 0 0 1 tx ty],
             * where tx and ty are the distances in x and y. */
            m.Translate(-20, 80);
            /* Concatenates the m  to the matrix3.
             * [matrix3] =  [matrix3] * [m]
             */
            matrix3.Concatenate(m);
            //Clear the canvas with white color.
            _graphics2D.Clear(Color.White);

            _graphics2D.AffineTransform = (matrix1);
            SolidBrush brush = new SolidBrush(fillColor);
            Pen pen = new Pen(redColor, 4);

            _graphics2D.SetPenAndBrush(pen, brush);
            _graphics2D.Draw(null, path);

            pen = new Pen(blackColor, 1);
            _graphics2D.DefaultPen = (pen);
            _graphics2D.DrawChars(font, fontSize, engText, 0, engText.Length, X, Y);



            _graphics2D.AffineTransform = (matrix2);

            pen = new Pen(greenColor, 4);

            _graphics2D.SetPenAndBrush(pen, brush);
            _graphics2D.Draw(null, path);
            pen = new Pen(blackColor, 1);
            _graphics2D.DefaultPen = (pen);
            _graphics2D.DrawChars(font, fontSize, engText, 0, engText.Length, X, Y);


            _graphics2D.AffineTransform = (matrix3);

            pen = new Pen(blueColor, 4);

            _graphics2D.SetPenAndBrush(pen, brush);
            _graphics2D.Draw(null, path);
            pen = new Pen(blackColor, 1);
            _graphics2D.DefaultPen = (pen);
            _graphics2D.DrawChars(font, fontSize, engText, 0, engText.Length, X, Y);
            Invalidate();
        }

        /*
         * varialbes for beziers.
         */
        /**
         * The animation thread.
         */
        private Thread thread;
        bool drawn;
        /**
         * The random number generator.
         */
        Random random = new Random();
        /**
         * The animated path
         */
        Path path = new Path();
        /**
         * Red brush used to fill the path.
         */
        SolidBrush brush = new SolidBrush(Color.Red);
        private static int NUMPTS = 6;
        private int[] animpts = new int[NUMPTS * 2];
        private int[] deltas = new int[NUMPTS * 2];
        long startt, endt;

        /**
     * Generates new points for the path.
     */
        private void Animate(int[] pts, int[] deltas, int i, int limit)
        {
            int newpt = pts[i] + deltas[i];
            if (newpt <= 0)
            {
                newpt = -newpt;
                deltas[i] = (random.Next() & 0x00000003) + 2;
            }
            else if (newpt >= limit)
            {
                newpt = 2 * limit - newpt;
                deltas[i] = -((random.Next() & 0x00000003) + 2);
            }
            pts[i] = newpt;
        }

        /**
         * Resets the animation data.
         */
        private void Reset(int w, int h)
        {
            for (int i = 0; i < animpts.Length; i += 2)
            {
                animpts[i + 0] = (random.Next() & 0x00000003) * w / 2;
                animpts[i + 1] = (random.Next() & 0x00000003) * h / 2;
                deltas[i + 0] = (random.Next() & 0x00000003) * 6 + 4;
                deltas[i + 1] = (random.Next() & 0x00000003) * 6 + 4;
                if (animpts[i + 0] > w / 2)
                {
                    deltas[i + 0] = -deltas[i + 0];
                }
                if (animpts[i + 1] > h / 2)
                {
                    deltas[i + 1] = -deltas[i + 1];
                }
            }
        }

        /**
         * Sets the points of the path and draws and fills the path.
         */
        private void DrawDemo(int w, int h)
        {
            for (int i = 0; i < animpts.Length; i += 2)
            {
                Animate(animpts, deltas, i + 0, w);
                Animate(animpts, deltas, i + 1, h);
            }
            //Generates the new pata data.
            path.Reset();
            int[] ctrlpts = animpts;
            int len = ctrlpts.Length;
            int prevx = ctrlpts[len - 2];
            int prevy = ctrlpts[len - 1];
            int curx = ctrlpts[0];
            int cury = ctrlpts[1];
            int midx = (curx + prevx) / 2;
            int midy = (cury + prevy) / 2;
            path.MoveTo(midx, midy);
            for (int i = 2; i <= ctrlpts.Length; i += 2)
            {
                int x1 = (curx + midx) / 2;
                int y1 = (cury + midy) / 2;
                prevx = curx;
                prevy = cury;
                if (i < ctrlpts.Length)
                {
                    curx = ctrlpts[i + 0];
                    cury = ctrlpts[i + 1];
                }
                else
                {
                    curx = ctrlpts[0];
                    cury = ctrlpts[1];
                }
                midx = (curx + prevx) / 2;
                midy = (cury + prevy) / 2;
                int x2 = (prevx + midx) / 2;
                int y2 = (prevy + midy) / 2;
                path.CurveTo(x1, y1, x2, y2, midx, midy);
            }
            path.ClosePath();
            // clear the clipRect area before production

            _graphics2D.Clear(Color.White);
            _graphics2D.Fill(brush, path);
            Invalidate();
        }

        public void Run()
        {
            Thread me = Thread.CurrentThread;

            if (!drawn)
            {
                lock (this)
                {
                    _graphics2D.Clear(Color.White);
                    _graphics2D.Fill(brush, path);
                    Invalidate();
                    drawn = true;
                }
            }
            while (thread == me)
            {
                DrawDemo(100, 100);
            }
            thread = null;
        }
        private void beziersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphics2D.Reset();
            Reset(100, 100);
            thread = new Thread(Run);
            thread.Start();
        }

        private void fontTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] fontFileName = {
                "arial.fon",
                "courier.fon",
                "elephant.fon",
                "impact.fon",
                "georgia.fon",
                "rockwell.fon",
                "roman.fon",
                "serif.fon",
                "verdana.fon",
                "youyuan.fon",
                "xinwei.fon",
                "xinsong.fon",
                "xingkai.fon",
                "songti.fon",
                "lishu.fon",
                "fangsong.fon",
                "heiti.fon"
            };
            //Clear the canvas with white color.
            _graphics2D.Clear(Color.White);
            char[] longLine = null;
            char[] errorLine = "Error! ".ToCharArray();
            FontEx font = FontEx.GetSystemFont();
            try
            {
                FileStream fconn;
                _graphics2D.SetPenAndBrush(new Pen(Color.Black, 1), new SolidBrush(Color.Black));
                String fontName;
                for (int i = 0; i < fontFileName.Length; i++)
                {
                    fontName = @"D:\CVSMapDigit\DotNetMapDigitLib\Graphics2DExample\fonts\" + fontFileName[i];
                    fconn = new FileStream(fontName, FileMode.Open);
                    font = new FontEx(fconn);
                    longLine = font.Name.ToCharArray();
                    _graphics2D.DrawChars(font, 20, longLine, 0, longLine.Length, 20, 40 + 20 * i);
                    fconn.Close();
                }
            }
            catch (Exception ioe)
            {
                _graphics2D.DrawChars(font, 16, errorLine, 0, errorLine.Length, 10, 20);
            }

            Invalidate();
        }
    }
}
