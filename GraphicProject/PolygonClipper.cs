using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace GraphicProject
{
    class PolygonClipper
    {

        private static int[] qx4 = new int[50];
        private static int[] qy4 = new int[50];
        private static int polylength;
        public static void Sutherland_Hodgmancut(Graphics g, int[] px, int[] py, int polycount, int[] rectX, int[] rectY, Color color)
        {
            int XL = rectX[0];
            int XR = rectX[1];
            int YT = rectY[0];
            int YB = rectY[1];
            px[polycount] = px[0];
            py[polycount] = py[0];
            int code1, code2;
            int k = 0;
            int[] qx1 = new int[50];
            int[] qy1 = new int[50];
            int[] qx2 = new int[50];
            int[] qy2 = new int[50];
            int[] qx3 = new int[50];
            int[] qy3 = new int[50];
            for (int i = 0; i < polycount; i++)
            {

                int c = 0;
                if (px[i] < XL)
                    c = 1;
                else if (px[i] > XL)
                    c = 0;
                code1 = c;
                c = 0;
                if (px[i + 1] < XL)
                    c = 1;
                else if (px[i + 1] > XL)
                    c = 0;
                code2 = c;
                if (code1 != 0 && code2 == 0)
                {
                    qx1[k] = XL;
                    qy1[k] = py[i] + (py[i + 1] - py[i]) * (XL - px[i]) / (px[i + 1] - px[i]);
                    qx1[k + 1] = px[i + 1];
                    qy1[k + 1] = py[i + 1];
                    k = k + 2;
                }
                if (code1 == 0 && code2 == 0)
                {
                    if (k != 0)
                    {
                        qx1[k] = px[i + 1];
                        qy1[k] = py[i + 1];
                        k = k + 1;
                    }
                    else
                    {
                        qx1[k] = px[i];
                        qy1[k] = py[i];
                        qx1[k + 1] = px[i + 1];
                        qy1[k + 1] = py[i + 1];
                        k = k + 2;
                    }

                }
                if (code1 == 0 && code2 != 0)
                {
                    qx1[k] = XL;
                    qy1[k] = py[i] + (py[i + 1] - py[i]) * (XL - px[i]) / (px[i + 1] - px[i]);
                    k = k + 1;

                }
            }
            qx1[k] = qx1[0];
            qy1[k] = qy1[0];
            polycount = k;
            k = 0;
            for (int i = 0; i < polycount; i++)
            {

                int c = 0;
                if (qy1[i] < YT)
                    c = 8;
                else if (qy1[i] > YT)
                    c = 0;
                code1 = c;
                c = 0;
                if (qy1[i + 1] < YT)
                    c = 8;
                else if (qy1[i + 1] > YT)
                    c = 0;
                code2 = c;
                if (code1 != 0 && code2 == 0)
                {
                    qy2[k] = YT;
                    qx2[k] = qx1[i] + (YT - qy1[i]) * (qx1[i + 1] - qx1[i]) / (qy1[i + 1] - qy1[i]);
                    qx2[k + 1] = qx1[i + 1];
                    qy2[k + 1] = qy1[i + 1];
                    k = k + 2;
                }
                if (code1 == 0 && code2 == 0)
                {
                    if (k != 0)
                    {
                        qx2[k] = qx1[i + 1];
                        qy2[k] = qy1[i + 1];
                        k = k + 1;
                    }
                    else
                    {
                        qx2[k] = qx1[i];
                        qy2[k] = qy1[i];
                        qx2[k + 1] = qx1[i + 1];
                        qy2[k + 1] = qy1[i + 1];
                        k = k + 2;
                    }

                }
                if (code1 == 0 && code2 != 0)
                {
                    qy2[k] = YT;
                    qx2[k] = qx1[i] + (YT - qy1[i]) * (qx1[i + 1] - qx1[i]) / (qy1[i + 1] - qy1[i]);
                    k = k + 1;
                }
            }
            qx2[k] = qx2[0];
            qy2[k] = qy2[0];
            polycount = k;
            k = 0;
            for (int i = 0; i < polycount; i++)
            {

                int c = 0;
                if (qx2[i] < XR)
                    c = 0;
                else if (qx2[i] > XR)
                    c = 2;
                code1 = c;
                c = 0;
                if (qx2[i + 1] < XR)
                    c = 0;
                else if (qx2[i + 1] > XR)
                    c = 2;
                code2 = c;
                if (code1 != 0 && code2 == 0)
                {
                    qx3[k] = XR;
                    qy3[k] = qy2[i] + (qy2[i + 1] - qy2[i]) * (XR - qx2[i]) / (qx2[i + 1] - qx2[i]);
                    qx3[k + 1] = qx2[i + 1];
                    qy3[k + 1] = qy2[i + 1];
                    k = k + 2;
                }
                if (code1 == 0 && code2 == 0)
                {
                    if (k != 0)
                    {
                        qx3[k] = qx2[i + 1];
                        qy3[k] = qy2[i + 1];
                        k = k + 1;
                    }
                    else
                    {
                        qx3[k] = qx2[i];
                        qy3[k] = qy2[i];
                        qx3[k + 1] = qx2[i + 1];
                        qy3[k + 1] = qy2[i + 1];
                        k = k + 2;
                    }

                }
                if (code1 == 0 && code2 != 0)
                {
                    qx3[k] = XR;
                    qy3[k] = qy2[i] + (qy2[i + 1] - qy2[i]) * (XR - qx2[i]) / (qx2[i + 1] - qx2[i]);
                    k = k + 1;
                }
            }
            qx3[k] = qx3[0];
            qy3[k] = qy3[0];
            polycount = k;
            k = 0;
            for (int i = 0; i < polycount; i++)
            {

                int c = 0;
                if (qy3[i] < YB)
                    c = 0;
                else if (qy3[i] > YB)
                    c = 4;
                code1 = c;
                c = 0;
                if (qy3[i + 1] < YB)
                    c = 0;
                else if (qy3[i + 1] > YB)
                    c = 4;
                code2 = c;
                if (code1 != 0 && code2 == 0)
                {
                    qy4[k] = YB;
                    qx4[k] = qx3[i] + (YB - qy3[i]) * (qx3[i + 1] - qx3[i]) / (qy3[i + 1] - qy3[i]);
                    qx4[k + 1] = qx3[i + 1];
                    qy4[k + 1] = qy3[i + 1];
                    k = k + 2;
                }
                if (code1 == 0 && code2 == 0)
                {
                    if (k != 0)
                    {
                        qx4[k] = qx3[i + 1];
                        qy4[k] = qy3[i + 1];
                        k = k + 1;
                    }
                    else
                    {
                        qx4[k] = qx3[i];
                        qy4[k] = qy3[i];
                        qx4[k + 1] = qx3[i + 1];
                        qy4[k + 1] = qy3[i + 1];
                        k = k + 2;
                    }

                }
                if (code1 == 0 && code2 != 0)
                {
                    qy4[k] = YB;
                    qx4[k] = qx3[i] + (YB - qy3[i]) * (qx3[i + 1] - qx3[i]) / (qy3[i + 1] - qy3[i]);
                    k = k + 1;
                }
            }
            qx4[k] = qx4[0];
            qy4[k] = qy4[0];
            polycount = k;
            Pen pen1 = new Pen(color, 2);
            for (int j = 0; j < polycount; j++)
            {
                Point p1 = new Point();
                Point p2 = new Point();
                p1.X = qx4[j];
                p1.Y = qy4[j];
                p2.X = qx4[j + 1];
                p2.Y = qy4[j + 1];
                g.DrawLine(pen1, p1, p2);
            }
            polylength = polycount;
        }
    }
}
