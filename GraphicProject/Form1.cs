using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicProject
{
    public partial class Form1 : Form
    {

        String penType = " ";
        Bitmap mainBitmap;// the main bitmap of window
        Point startPoint,endPoint;
        Color myColor = Color.Red;
        public Form1()
        {
            InitializeComponent();
            mainBitmap = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            Graphics.FromImage(mainBitmap).Clear(Color.White);
            pictureBox1.Image = mainBitmap;
        }

    


        public void setPixel(int x,int y,Color c)
        {
            mainBitmap.SetPixel(x, y, c);
            pictureBox1.Image = mainBitmap;
        }

        //polygon
        Point previewPoint;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {          
            if (penType == "drawPolygon")
            {
                Geometry.polygonPointSets.Add(new Point(e.X, e.Y));
                if(Geometry.polygonPointSets.Count>=2)
                {
                    Geometry.drawLine(previewPoint, new Point(e.X, e.Y),myColor,mainBitmap);
                    pictureBox1.Refresh();
                }
                if (e.Button == MouseButtons.Right)
                {
                    Geometry.drawLine(Geometry.polygonPointSets[0], new Point(e.X, e.Y), myColor, mainBitmap);
                    pictureBox1.Refresh();
                    penType = "";                   
                }
                previewPoint.X = e.X;
                previewPoint.Y = e.Y;
            }            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {          
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isClick = true;
            startPoint.X = e.X;
            startPoint.Y = e.Y;
            if(penType=="curve")
            {
                if (Curve.no_of_points > 3)
                {
                    Curve.pt[0] = Curve.pt[1];
                    Curve.pt[1] = Curve.pt[2];
                    Curve.pt[2] = Curve.pt[3];
                    Curve.pt[3].setxy(e.X, e.Y);
                    double temp = Math.Sqrt(Math.Pow(Curve.pt[2].x - Curve.pt[1].x, 2F) + Math.Pow(Curve.pt[2].y - Curve.pt[1].y, 2F));
                    int interpol = System.Convert.ToInt32(temp);
                    Curve.bsp(Curve.pt[0], Curve.pt[1], Curve.pt[2], Curve.pt[3], interpol);
                    int i;
                    int width = 2;
                    Graphics g = pictureBox1.CreateGraphics();
                    Color cl = Color.Blue;
                    int x, y;
                    // the lines are drawn
                    for (i = 0; i <= interpol - 1; i++)
                    {
                        x = System.Convert.ToInt32(Curve.splinex[i]);
                        y = System.Convert.ToInt32(Curve.spliney[i]);
                        //g.DrawLine(new Pen(cl, width), x - 1, y, x + 1, y);
                        //g.DrawLine(new Pen(cl, width), x, y - 1, x, y + 1);
                        //Geometry.drawLine(new Point(x-1,y),new Point(x+1,y),myColor,mainBitmap);
                        //Geometry.drawLine(new Point(x , y-1), new Point(x , y + 1), myColor, mainBitmap);
                        mainBitmap.SetPixel(x, y, myColor);
                        pictureBox1.Refresh();
                    }
                }
                else
                {
                    Curve.pt[Curve.no_of_points].setxy(e.X, e.Y);
                }
                Curve.no_of_points = Curve.no_of_points + 1;
            }

        }


        bool isClick = false;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isClick&&(penType=="select"))
            {
                pictureBox1.Refresh();
                cloneZone = Geometry.makeRectangleFromPoints(startPoint, new Point(e.X, e.Y));
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawRectangle(new Pen(Color.Gray, (float)1.0), cloneZone);
            }
            switch (penType)
            {
                //case "drawLine":
                //    Geometry.drawLine(startPoint, new Point(e.X, e.Y), myColor, mainBitmap);

                //    Geometry.setPixel(new Point(e.X, e.Y), myColor, mainBitmap);
                //    pictureBox1.Refresh();
                //    break;
                //case "drawCircle":
                //    break;
                //case "drawEllipse":
                //    break;
                //case "drawPolygon":
                //    break;
                default:
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isClick = false;
            endPoint.X = e.X;
            endPoint.Y = e.Y;

            switch(penType)
            {
                case "drawLine":
                    Geometry.drawLine(startPoint, new Point(e.X, e.Y), myColor, mainBitmap);                 
                    pictureBox1.Refresh();
                    break;
                case "potline":
                    Geometry.potLine(startPoint, new Point(e.X, e.Y), myColor, mainBitmap);                 
                    pictureBox1.Refresh();
                    break;
                case "ddaline":
                    Geometry.ddaLine(startPoint, new Point(e.X, e.Y), myColor, mainBitmap);
                    pictureBox1.Refresh();
                    break;
                case "drawCircle":
                    Geometry.drawCircle(startPoint, endPoint, myColor, mainBitmap);
                    pictureBox1.Refresh();

                    break;
                case "drawEllipse":
                    Geometry.drawEllipse(startPoint, endPoint, myColor, mainBitmap);
                    pictureBox1.Refresh();
                    break;
                case "drawPolygon":


                    break;
                case "curve":

                    mainBitmap.SetPixel(e.X, e.Y, myColor);
                    pictureBox1.Refresh();


                    break;
                default:
                    break;
            }
            
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            penType = "drawLine";
            textBox1.Text = "鼠标左键单击按下，移动一段距离后弹起画线";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            penType = "drawCircle";
            textBox1.Text = "鼠标左键单击按下，取得圆心，移动一段距离后弹起取得圆半径，画圆";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            penType = "drawEllipse";
            textBox1.Text = "鼠标左键单击按下，取得椭圆中心，移动一段距离后弹起取得椭圆长半轴和短半轴，画椭圆";
        }


        private void button4_Click(object sender, EventArgs e)
        {
            penType = "drawPolygon";
            textBox1.Text = "鼠标左键单击依次取得多边形定点，右键结束取点，开始画多边形";
            if (Geometry.polygonPointSets.Count>0)
            {
                Geometry.polygonPointSets.Clear();
            }
        }

        //填充多边形
        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "当屏幕中有多边形时可对其填充";
            if (Geometry.polygonPointSets.Count>=3)
            {
                Geometry.fillPolygon(Geometry.polygonPointSets, myColor, mainBitmap);
                pictureBox1.Refresh();
            }
        }
        //线段裁剪
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "任意画一条直线，再次单击“线段裁剪”按钮可对其按照裁剪框裁剪";
            //Graphics g = pictureBox1.CreateGraphics();
            //Pen p = new Pen(Color.Blue);
            Geometry.drawLine(new Point(Clip.left, Clip.top), new Point(Clip.right, Clip.top),myColor,mainBitmap);
            Geometry.drawLine( new Point(Clip.left, Clip.top), new Point(Clip.left, Clip.buttom), myColor, mainBitmap);
            Geometry.drawLine( new Point(Clip.right, Clip.top), new Point(Clip.right, Clip.buttom), myColor, mainBitmap);
            Geometry.drawLine( new Point(Clip.right, Clip.buttom), new Point(Clip.left, Clip.buttom), myColor, mainBitmap);
            pictureBox1.Refresh();
            //button1_Click(sender,e);
            //Geometry.drawLine(startPoint, endPoint,myColor, mainBitmap);
            pictureBox1.Refresh();
            Clip.CohenSutherland(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y,myColor,mainBitmap);
            pictureBox1.Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = "鼠标单击屏幕依次取点实现自由曲线绘制";
            penType = "curve";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text = "设置颜色";
            colorDialog1.ShowDialog();
            myColor = colorDialog1.Color;
           
        }

        List<Point> tranglePoints = new List<Point>() { new Point(219, 312), new Point(446, 312), new Point(333, 145) };
        //Point tranglePoint1 = new Point(219, 312);
        //Point tranglePoint2 = new Point(446, 312);
        //Point tranglePoint3 = new Point(333, 145);
        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "W、S、A、D按键实现上下左右移动；V、B实现放大缩小；Ｚ、Ｘ实现旋转";
            penType = "2d";
            mainBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(mainBitmap).Clear(Color.White);
            pictureBox1.Image = mainBitmap;
            pictureBox1.Refresh();
            Geometry.drawLine(tranglePoints[0], tranglePoints[1], myColor, mainBitmap);
            Geometry.drawLine(tranglePoints[1], tranglePoints[2], myColor, mainBitmap);
            Geometry.drawLine(tranglePoints[2], tranglePoints[0], myColor, mainBitmap);
            pictureBox1.Refresh();
            
        }

        //图形变换
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
                if (penType == "2d")
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            List<Point> p = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                p.Add(new Point(tranglePoints[i].X, tranglePoints[i].Y + 5));

                            }
                            tranglePoints = p;
                            button11_Click(sender, e);

                            break;
                        case Keys.D:
                            List<Point> pLeft = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                pLeft.Add(new Point(tranglePoints[i].X + 5, tranglePoints[i].Y));

                            }
                            tranglePoints = pLeft;
                            button11_Click(sender, e);
                            break;
                        case Keys.W:
                            List<Point> pUp = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                pUp.Add(new Point(tranglePoints[i].X, tranglePoints[i].Y - 5));

                            }
                            tranglePoints = pUp;
                            button11_Click(sender, e);

                            break;
                        case Keys.A:

                            List<Point> pRight = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                pRight.Add(new Point(tranglePoints[i].X - 5, tranglePoints[i].Y));

                            }
                            tranglePoints = pRight;
                            button11_Click(sender, e);
                            break;

                        case Keys.Z:
                            List<Point> pClockwise = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                pClockwise.Add(new Point((int)(tranglePoints[i].X * Math.Cos(Math.PI / 36) - tranglePoints[i].Y * Math.Sin(Math.PI / 36)), (int)(tranglePoints[i].X * Math.Sin(Math.PI / 36) + tranglePoints[i].Y * Math.Cos(Math.PI / 36))));
                            }
                            tranglePoints = pClockwise;
                            button11_Click(sender, e);
                            break;
                        case Keys.X:
                            List<Point> pCounterclockwise = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                pCounterclockwise.Add(new Point((int)(tranglePoints[i].X * Math.Cos(-Math.PI / 36) - tranglePoints[i].Y * Math.Sin(-Math.PI / 36)), (int)(tranglePoints[i].X * Math.Sin(-Math.PI / 36) + tranglePoints[i].Y * Math.Cos(-Math.PI / 36))));
                            }
                            tranglePoints = pCounterclockwise;
                            button11_Click(sender, e);
                            break;
                        case Keys.V:
                            List<Point> pZoomin = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                pZoomin.Add(new Point((int)(tranglePoints[i].X * 1.1), (int)(tranglePoints[i].Y * 1.1)));

                            }
                            tranglePoints = pZoomin;
                            button11_Click(sender, e);
                            break;
                        case Keys.B:
                            List<Point> pZoomout = new List<Point>();
                            for (int i = 0; i < tranglePoints.Count; i++)
                            {
                                pZoomout.Add(new Point((int)(tranglePoints[i].X * 0.9), (int)(tranglePoints[i].Y * 0.9)));

                            }
                            tranglePoints = pZoomout;
                            button11_Click(sender, e);
                            break;
                        default:
                            break;
                    }

                    Console.WriteLine();
                }

                if(penType=="3d")
                {
                    tr = new trans3(p2, p2.Length);
                    switch (e.KeyCode)
                    {
                        case Keys.U:
                            alf = -10;
                            tr.rotateMX(alf);
                            break;
                        case Keys.I:
                            alf = 10;
                            tr.rotateMX(alf);
                            break;
                        case Keys.H:
                            belta = -10;
                            tr.rotateMY(belta);
                            break;
                        case Keys.J:
                            belta = 10;
                            tr.rotateMY(belta);
                            break;
                        case Keys.K:
                            belta = -10;
                            tr.rotateMZ(belta);
                            break;
                        case Keys.L:
                            belta = 10;
                            tr.rotateMZ(belta);
                            break;
                        case Keys.R:
                            for (int i = 0; i < p1.Length; i++)
                            {
                                p2[i] = new point3(p1[i].x, p1[i].y, p1[i].z);
                            }
                            break;
                        default:
                            break;
                    }
                    pictureBox1.Refresh();
                    Graphics g = pictureBox1.CreateGraphics();
                    drawc(g);
                }
           // }
            //catch (Exception ec)
            //{

            //    MessageBox.Show(ec.Message, "超出画图边界！");
            //}
        }

        private void dRAWLINEToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //clone
        Rectangle cloneZone;
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "必须先选择操作范围,粘贴时鼠标先点击取点取得粘贴的位置,默认位置为左上角";
            try
            {
                Clipboard.SetImage(mainBitmap.Clone(cloneZone, mainBitmap.PixelFormat));
                startPoint.X = 0;
                startPoint.Y = 0;
            }
            catch
            {
                MessageBox.Show("请选择要复制的区域","剪贴板为空");
            }
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "必须先进行复制或剪贴操作";
            Graphics g = Graphics.FromImage(mainBitmap);   
            g.DrawImage(Clipboard.GetImage(),startPoint);         
            pictureBox1.Refresh();
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "鼠标左键按下，移动，然后弹起选择操作区域";
            penType = "select";
        }

        private void 剪贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "必须先选择操作范围,粘贴时鼠标先点击取点取得粘贴的位置,默认位置为左上角";
            try
            {
                Clipboard.SetImage(mainBitmap.Clone(cloneZone, mainBitmap.PixelFormat));
                Graphics g = Graphics.FromImage(mainBitmap);
                
                g.FillRectangle(Brushes.White, cloneZone);
                pictureBox1.Refresh();

            }
            catch
            {
                MessageBox.Show("请选择要剪贴的区域", "剪贴板为空");
            }
            startPoint.X = 0;
            startPoint.Y = 0;
        }


        bool besaved=false;
        private void sAVRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (besaved == false)
            {
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    string s = saveFileDialog1.FileName + ".bmp";
                    
                    mainBitmap.Save(s, System.Drawing.Imaging.ImageFormat.Bmp);
                    besaved = true;
                }
                
            }
        }

        private void oPENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                mainBitmap.Dispose();
                mainBitmap = new Bitmap(openFileDialog1.FileName);
                //bitG = Graphics.FromImage(bits);
                pictureBox1.Image = mainBitmap;
                pictureBox1.Refresh();


            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mainBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(mainBitmap).Clear(Color.White);
            pictureBox1.Image = mainBitmap;
            pictureBox1.Refresh();
            Geometry.polygonPointSets.Clear();
           
        }



        //3d transformation
        //
        public point3[] p1 = new point3[8];
        public point3[] p2 = new point3[8];
        public trans3 tr;
        public face[] f1 = new face[6];
        public double alf;
        public double belta;
        private void button10_Click(object sender, EventArgs e)
        {
            penType = "3d";
            textBox1.Text = "U、I实现绕X轴旋转；H、J实现绕Y周旋转；K、L实现绕Z轴旋转;再次单击‘自由变换’停止转动";

            double a = 80;//边长为2a
            p1[0] = new point3(-a, -a, -a);
            p1[1] = new point3(a, -a, -a);
            p1[2] = new point3(a, a, -a);
            p1[3] = new point3(-a, a, -a);
            p1[4] = new point3(-a, -a, a);
            p1[5] = new point3(a, -a, a);
            p1[6] = new point3(a, a, a);
            p1[7] = new point3(-a, a, a);
            f1[0] = new face(4); f1[0].setedge(0, 0); f1[0].setedge(1, 3); f1[0].setedge(2, 2); f1[0].setedge(3, 1);//后面
            f1[1] = new face(4); f1[1].setedge(0, 4); f1[1].setedge(1, 5); f1[1].setedge(2, 6); f1[1].setedge(3, 7);//前面
            f1[2] = new face(4); f1[2].setedge(0, 0); f1[2].setedge(1, 4); f1[2].setedge(2, 7); f1[2].setedge(3, 3); //左面
            f1[3] = new face(4); f1[3].setedge(0, 1); f1[3].setedge(1, 2); f1[3].setedge(2, 6); f1[3].setedge(3, 5); //右面
            f1[4] = new face(4); f1[4].setedge(0, 3); f1[4].setedge(1, 7); f1[4].setedge(2, 6); f1[4].setedge(3, 2);//顶面
            f1[5] = new face(4); f1[5].setedge(0, 0); f1[5].setedge(1, 1); f1[5].setedge(2, 5); f1[5].setedge(3, 4); ;//底面

            for (int i = 0; i < p1.Length; i++)
            {
                p2[i] = new point3(p1[i].x, p1[i].y, p1[i].z);

            }
            //pictureBox1.Refresh();
            Graphics g = pictureBox1.CreateGraphics();
            drawc(g);
            
        }

        List<Color> colorSets = new List<Color>() { Color.LightPink, Color.Yellow, Color.Purple, Color.Blue, Color.Green, Color.Red };
        public void drawc(Graphics g)
        {
            mainBitmap = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            pictureBox1.Image = mainBitmap;


            Point temp = new Point();
            Point temp2 = new Point();
            Pen mypen = new Pen(Brushes.Red);
            List<Point> l = new List<Point>();//fill face 

            List<FillFace3D> fillFace = new List<FillFace3D>();//record the 6 faces and it's points
            for (int nface = 0; nface < 6; nface++)
            {
                FillFace3D face3D = new FillFace3D();
                for (int nedge = 0; nedge < f1[nface].m_enum; nedge++)
                {
                    face3D.pointSets3D.Add(new point3(p2[f1[nface].p[nedge]].x, p2[f1[nface].p[nedge]].y, p2[f1[nface].p[nedge]].z));

                    temp.X = (int)p2[f1[nface].p[nedge]].x + 300;
                    temp.Y = (int)p2[f1[nface].p[nedge]].y + 300;
                    if (nedge == 0)
                    {
                        temp2 = temp;
                    }
                    else
                    {
                       // g.DrawLine(mypen, temp2, temp);
                        temp2 = temp;
                        if (nedge == 3)
                        {
                            temp.X = (int)p2[f1[nface].p[0]].x + 300;
                            temp.Y = (int)p2[f1[nface].p[0]].y + 300;
                           // g.DrawLine(mypen, temp2, temp);
                        }
                    }
                    //l.Add(temp2);
                    face3D.pointSets2D.Add(temp2);
                }
                //Geometry.fillPolygon(l, colorSets[nface], mainBitmap);
                //pictureBox1.Refresh();
                //l.Clear();          
                face3D.calcuDepth();
                face3D.faceColor = colorSets[nface];   
                fillFace.Add(face3D);


            }

            fillFace = getOrded(fillFace);

            for (int i=0; i<6;i++)
            {
                Geometry.fillPolygon(fillFace[i].pointSets2D, fillFace[i].faceColor, mainBitmap);
            }
             pictureBox1.Refresh();

            //List<Point> cubicPointSets=new List<Point>();

            //for(int i=0;i<f1.Length;i++)
            //{
            //    Point tmp;
            //    foreach(int j in f1[i].p)
            //    {
            //        cubicPointSets.Add(new Point(p2[j]));
            //    }
            //    Geometry.fillPolygon();
            //}


        }

        public void fill3DCubic(List<FillFace3D> fillFace,Bitmap mainBitmap,Color myColor)
        {
            foreach(FillFace3D f3 in fillFace)
            {
                Geometry.fillPolygon(f3.pointSets2D, myColor, mainBitmap);
            }

        }


        public List<FillFace3D> getOrded(List<FillFace3D> fillFace)
        {
            List<FillFace3D> orderedFillingFace = new List<FillFace3D>();
            if (fillFace.Count==6)
            {
                
                for (int j = 0; j < 6; j++)
                {
                    FillFace3D f3 = fillFace[0];
                    for (int i = 0; i < fillFace.Count; i++)
                    {
                        if (fillFace[i].depth < f3.depth)
                        {
                            f3 = fillFace[i];
                        }
                    }
                    orderedFillingFace.Add(f3);
                    fillFace.Remove(f3);
                }
            }
            return orderedFillingFace;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            penType = "potline";
            textBox1.Text = "鼠标左键单击按下，移动一段距离后弹起画线";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                button10_Click(sender, e);
                timer1.Interval = 50;
                timer1.Enabled = true;

            }
            else if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
            }
        }



        int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;

            // Form1_KeyDown(sender, new KeyEventArgs(Keys.U));
            if (count % 45 < 15)
                Form1_KeyDown(sender, new KeyEventArgs(Keys.H));
            else if ((count % 45 >= 15) && (count % 45 < 30))
                Form1_KeyDown(sender, new KeyEventArgs(Keys.K));
            else
                Form1_KeyDown(sender, new KeyEventArgs(Keys.U));
            if (count == 45)
                count = 0;

            //Form1_KeyDown(sender, new KeyEventArgs(Keys.K));
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
            textBox1.Text = "任意画一个多边形，再次单击“多边形裁剪”按钮可对其按照裁剪框裁剪";
            //Graphics g = pictureBox1.CreateGraphics();
            //Pen p = new Pen(Color.Blue);
            Geometry.ddaLine(new Point(Clip.left, Clip.top), new Point(Clip.right, Clip.top), myColor, mainBitmap);
            Geometry.ddaLine(new Point(Clip.left, Clip.top), new Point(Clip.left, Clip.buttom), myColor, mainBitmap);
            Geometry.ddaLine(new Point(Clip.right, Clip.top), new Point(Clip.right, Clip.buttom), myColor, mainBitmap);
            Geometry.ddaLine(new Point(Clip.right, Clip.buttom), new Point(Clip.left, Clip.buttom), myColor, mainBitmap);

            pictureBox1.Refresh();
            //if (penType == "polygonClipper")
            {
                if(Geometry.polygonPointSets.Count>2)
                {
                    Graphics g = Graphics.FromImage(mainBitmap);
                    int[] ptX = new int[50];
                    int[] ptY = new int[50];
                    int[] rectX = new int[2] { 200, 300 };
                    int[] rectY = new int[2] { 200, 300 };
                    for (int i=0;i<Geometry.polygonPointSets.Count;i++)
                    {
                        ptX[i] = Geometry.polygonPointSets[i].X;
                        ptY[i] = Geometry.polygonPointSets[i].Y;

                    }
                    PolygonClipper.Sutherland_Hodgmancut(g, ptX, ptY, Geometry.polygonPointSets.Count, rectX, rectY, Color.Blue);
                    pictureBox1.Refresh();
                }
                else
                {
                    MessageBox.Show("请先画多边形");
                }
            }
            penType = "polygonClipper";
            //button1_Click(sender,e);
            //Geometry.drawLine(startPoint, endPoint,myColor, mainBitmap);
            // pictureBox1.Refresh();
            //Clip.CohenSutherland(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, myColor, mainBitmap);
            //pictureBox1.Refresh();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            penType = "ddaline";
            textBox1.Text = "鼠标左键单击按下，移动一段距离后弹起画线";
        }

    







    }
}
