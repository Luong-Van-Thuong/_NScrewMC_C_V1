using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public class Line
    {
        public Point StartPoint { get; private set; }
        public Point EndPoint { get; private set; }
        private PictureBox pictureBox;
        private const int HandleSize = 10;
        private const int MoveThreshold = 10; // Điều chỉnh để phù hợp với độ gần cần thiết
        private bool isResizing;
        private bool isMoving;
        private bool resizingStartPoint;
        private Point mouseDownLocation;
        private Point initialStartPoint;
        private Point initialEndPoint;

        public Line(Point start, Point end, PictureBox pb)
        {
            isCleared = false;
            StartPoint = start;
            EndPoint = end;
            pictureBox = pb;
            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            DrawLine(e.Graphics);
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLocation = e.Location;
                if (IsNearPoint(e.Location))
                {
                    StartResizing(e.Location);
                }
                else if (IsNearLine(e.Location))
                {
                    isMoving = true;
                    initialStartPoint = StartPoint;
                    initialEndPoint = EndPoint;
                }
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                Resize(e.Location);
                pictureBox.Invalidate();
            }
            else if (isMoving)
            {
                Point delta = new Point(e.Location.X - mouseDownLocation.X, e.Location.Y - mouseDownLocation.Y);
                Move(delta);

                pictureBox.Invalidate();
            }

        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                EndResizing();
            }
            else if (isMoving)
            {
                isMoving = false;
            }
        }

        public void DrawLine(Graphics g)
        {
            g.DrawLine(Pens.Lime, StartPoint, EndPoint);
            DrawHandle(g, StartPoint);
            DrawHandle(g, EndPoint);
        }

        private void DrawHandle(Graphics g, Point p)
        {
            g.DrawLine(Pens.Red, p.X - HandleSize / 2, p.Y, p.X + HandleSize / 2, p.Y); // Ngang
            g.DrawLine(Pens.Red, p.X, p.Y - HandleSize / 2, p.X, p.Y + HandleSize / 2); // Dọc
        }

        public bool IsNearPoint(Point p)
        {
            return Distance(p, StartPoint) < HandleSize || Distance(p, EndPoint) < HandleSize;
        }

        public bool IsNearLine(Point p)
        {
            double distance = Math.Abs((EndPoint.Y - StartPoint.Y) * p.X - (EndPoint.X - StartPoint.X) * p.Y + EndPoint.X * StartPoint.Y - EndPoint.Y * StartPoint.X) /
                              Math.Sqrt(Math.Pow(EndPoint.Y - StartPoint.Y, 2) + Math.Pow(EndPoint.X - StartPoint.X, 2));
            return distance < MoveThreshold;
        }

        public void StartResizing(Point p)
        {
            if (Distance(p, StartPoint) < HandleSize)
            {
                isResizing = true;
                resizingStartPoint = true;
            }
            else if (Distance(p, EndPoint) < HandleSize)
            {
                isResizing = true;
                resizingStartPoint = false;
            }
        }

        public void Resize(Point newLocation)
        {
            if (isResizing)
            {
                if (resizingStartPoint)
                    StartPoint = newLocation;
                else
                    EndPoint = newLocation;
            }
        }

        public void Move(Point delta)
        {
            StartPoint = new Point(initialStartPoint.X + delta.X, initialStartPoint.Y + delta.Y);
            EndPoint = new Point(initialEndPoint.X + delta.X, initialEndPoint.Y + delta.Y);
        }

        public void EndResizing()
        {
            isResizing = false;
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public void GetPoint(out Point _Start, out Point _end)
        {
            if (StartPoint.X < EndPoint.X)
            {
                _Start = StartPoint;
                _end = EndPoint;
            }
            else
            {
                _Start = StartPoint;
                _end = EndPoint;
            }
        }
        public bool isCleared = false;
        public void Clear()
        {
            if (isCleared) return; // Đảm bảo không thực thi nhiều lần
            isCleared = true;

            pictureBox.Paint -= PictureBox_Paint;
            pictureBox.MouseDown -= PictureBox_MouseDown;
            pictureBox.MouseMove -= PictureBox_MouseMove;
            pictureBox.MouseUp -= PictureBox_MouseUp;
            pictureBox.Invalidate();
        }
    }
}
