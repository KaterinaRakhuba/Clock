using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AnalogClock
{
    public partial class Form1 : Form
    {
        Timer mTimer = new Timer();
        delegate void Digits(Graphics g, SolidBrush solidBrush, int x, int y);

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            mTimer.Interval = 1000;
            mTimer.Tick += new EventHandler(onTimer);
            mTimer.Start();
            Text = "Analog clock";
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            SolidBrush red = new SolidBrush(Color.Red);
            SolidBrush green = new SolidBrush(Color.Green);
            SolidBrush blue = new SolidBrush(Color.Blue);
            SolidBrush white = new SolidBrush(Color.White);
            SolidBrush black = new SolidBrush(Color.Black);
            SolidBrush seaGreen = new SolidBrush(Color.MediumSeaGreen);
            SolidBrush lightGray = new SolidBrush(Color.LightGray);
            InitializeTransform(g);

            int x = -33, y = 30;
            for (int i = 0; i < 6; i++)
            {
                g.FillRectangle(seaGreen, x, y, 9, 15);
                ChoiseDigit(FindDigit(i),g, lightGray,x,y);

                if (i==1 || i==3)
                {
                    x += 10;
                    g.FillRectangle(black, x, y + 14, 1, 1);
                    x += 3;
                }
                else
                {
                    x += 11;
                }
            }
           
            //draw border
            for (int i = 0; i < 120; i++)
            {
                g.RotateTransform(5.0f);
                g.FillRectangle(black, 90, -5, 10, 10);
            }
            //draw hour mark
            for (int i = 0; i < 12; i++)
            {
                g.RotateTransform(30.0f);
                g.FillRectangle(white, 85, -5, 10, 10);
            }
            //draw minute mark
            for (int i = 0; i < 60; i++)
            {
                g.RotateTransform(6.0f);
                g.FillRectangle(black, 85, -10, 5, 1);
            }

            //get current time
            DateTime nowDateTime = DateTime.Now;
            int secondInt = nowDateTime.Second;
            int minuteInt = nowDateTime.Minute;
            int hourInt = nowDateTime.Hour % 12;
            InitializeTransform(g);
            //hour hand draw
            g.RotateTransform((hourInt * 30) + (minuteInt / 2));
            DrawHand(g, blue, 70, false);
            InitializeTransform(g);
            //minute hand draw
            g.RotateTransform(minuteInt * 6);
            DrawHand(g, red, 100, false);
            InitializeTransform(g);
            //second hand draw
            g.RotateTransform(secondInt * 6);
            DrawHand(g, green, 100, true);

            red.Dispose();
            green.Dispose();
            blue.Dispose();
            white.Dispose();
            black.Dispose();
        }

        private void DrawHand(Graphics g, SolidBrush solidBrush, int length, bool seen)
        {
            Point[] points = new Point[4];
            points[0].X = 0;
            points[0].Y = -length;
            points[1].X = (seen) ? -2 : -10;
            points[1].Y = 0;
            points[2].X = 0;
            points[2].Y = (seen) ? 2 : 10;
            points[3].X = (seen) ? 2 : 10;
            points[3].Y = 0;
            g.FillPolygon(solidBrush, points);
        }

        private void InitializeTransform(Graphics g)
        {
            g.ResetTransform();
            g.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            float scale = System.Math.Min(ClientSize.Width, ClientSize.Height) / 200.0f;
            g.ScaleTransform(scale, scale);
        }

        private void onTimer(object sender, EventArgs e)
        {
            Invalidate();
        }

        private int FindDigit(int i)
        {
            DateTime nowDateTime = DateTime.Now;
            int secondInt = nowDateTime.Second;
            int minuteInt = nowDateTime.Minute;
            int hourInt = nowDateTime.Hour;
            return i == 0 ? hourInt / 10
                : i == 1 ? hourInt % 10
                : i == 2 ? minuteInt / 10
                : i == 3 ? minuteInt % 10
                : i == 4 ? secondInt / 10
                : i == 5 ? secondInt % 10
                : 0;
        }

        private void ChoiseDigit(int ind, Graphics g, SolidBrush solidBrush, int x, int y)
        {
            Digits choise =null;
            if (ind == 1)
                choise = DigitOne;
            if (ind == 2)
                choise = DigitTwo;
            if (ind == 3)
                choise = DigitThree;
            if (ind == 4)
                choise = DigitFour;
            if (ind == 5)
                choise = DigitFive;
            if (ind == 6)
                choise = DigitSix;
            if (ind == 7)
                choise = DigitSeven;
            if (ind == 8)
                choise = DigitEight;
            if (ind == 9)
                choise = DigitNine;
            if (ind == 0)
                choise = DigitZero;
            choise(g, solidBrush, x, y);
        }
        private void DigitOne(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x, y, 6, 15);
        }
        private void DigitTwo(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x, y + 3, 6, 3);
            g.FillRectangle(solidBrush, x + 3, y + 9, 6, 3);
        }
        private void DigitThree(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x, y + 3, 6, 3);
            g.FillRectangle(solidBrush, x, y + 9, 6, 3);
        }
        private void DigitFour(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x + 3, y, 3, 6);
            g.FillRectangle(solidBrush, x, y + 9, 6, 6);
        }
        private void DigitFive(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x + 3, y + 3, 6, 3);
            g.FillRectangle(solidBrush, x, y + 9, 6, 3);
        }
        private void DigitSix(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x + 3, y + 3, 6, 3);
            g.FillRectangle(solidBrush, x + 3, y + 9, 6, 3);
        }
        private void DigitSeven(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x, y + 3, 6, 12);
        }
        private void DigitEight(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x + 3, y + 3, 3, 3);
            g.FillRectangle(solidBrush, x + 3, y + 9, 3, 3);
        }
        private void DigitNine(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x + 3, y + 3, 3, 3);
            g.FillRectangle(solidBrush, x, y + 9, 6, 3);
        }
        private void DigitZero(Graphics g, SolidBrush solidBrush, int x, int y)
        {
            g.FillRectangle(solidBrush, x + 3, y + 3, 3, 9);
        }
    }
}
