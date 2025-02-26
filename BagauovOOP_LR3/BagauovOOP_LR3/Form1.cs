﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BagauovOOP_LR3
{
    public partial class Form1 : Form
    {
        private CircleContainer circles = new CircleContainer();
        private Point startPoint;
        private bool selecting = false;
        private Rectangle selectionRectangle;
        private List<CCircle> selectedCircles = new List<CCircle>();
        private bool mouseMoved = false;

        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint_DrawCircles);
            this.MouseClick += new MouseEventHandler(Form1_MouseClick);
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);  
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);  
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Form1_Paint_DrawCircles(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            List<CCircle> allCircles = circles.GetAll();


            for (int i = 0; i < allCircles.Count; i++)
            {
                CCircle circle = allCircles[i];

                float penThickness = 3.0f; 
                Pen bluePen = new Pen(Color.Blue, penThickness);
                
                if (selectedCircles.Contains(circle))
                {
                    g.DrawEllipse(bluePen, circle.X - circle.Radius, circle.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
                    g.FillEllipse(Brushes.Red, circle.X - circle.Radius, circle.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);

                }
                else
                    g.DrawEllipse(Pens.Black, circle.X - circle.Radius, circle.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
            }
            if (selecting)
            {
                Pen pen = new Pen(Color.Blue);
                e.Graphics.DrawRectangle(pen, selectionRectangle);
                pen.Dispose();

            }

        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (selecting)
            {
                selectedCircles.Clear();
                circles.Add(new CCircle(e.X, e.Y, 30));
                this.Invalidate();
            }


        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                selecting = true;
                mouseMoved = false;
                selectionRectangle = new Rectangle(startPoint.X, startPoint.Y, 0, 0);
                this.Invalidate();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selecting)
            {
                mouseMoved = true;
                int x = Math.Min(startPoint.X, e.X);
                int y = Math.Min(startPoint.Y, e.Y);
                int width = Math.Abs(e.X - startPoint.X);
                int height = Math.Abs(e.Y - startPoint.Y);

                selectionRectangle = new Rectangle(x, y, width, height);
                
                this.Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selecting = false;
                if (!mouseMoved)
                {
                    circles.Add(new CCircle(e.X, e.Y, 30));
                }
                else
                {
                    selectedCircles.Clear();
                    List<CCircle> allCircles = circles.GetAll();
                    for (int i = 0; i < allCircles.Count(); i++)
                    {
                        CCircle circle = allCircles[i];

                        Rectangle circleBounds = new Rectangle(circle.X - circle.Radius, circle.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
                        if (selectionRectangle.IntersectsWith(circleBounds))
                        {
                            selectedCircles.Add(circle);

                        }
                    }
                }


                this.Invalidate();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                circles.GetAll().Clear();
                this.Invalidate();
            }
        }
    }


    public class CCircle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; private set; }

        public CCircle(int x, int y, int radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }
    }


    public class CircleContainer
    {
        private List<CCircle> circles = new List<CCircle>();

        public void Add(CCircle circle)
        {
            circles.Add(circle);
        }

        public List<CCircle> GetAll()
        {
            return circles;
        }

       
    }
}