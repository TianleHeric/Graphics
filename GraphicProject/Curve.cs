using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicProject
{
    public static class Curve
    {
        public static int no_of_points = 1;
        public static double[] splinex = new double[1001];
        public static double[] spliney = new double[1001];
        public static point[] pt = new point[6];   
        public static int[] a1 = new int[12];
        public static int[] b1 = new int[12];
        public struct point
        {
            public int x;
            public int y;

            public void setxy(int i, int j)
            {
                x = i;
                y = j;
            }
        }     
        public static void bsp(point p1, point p2, point p3, point p4, int divisions)
        {
            double[] a = new double[5];
            double[] b = new double[5];
            a[0] = (-p1.x + 3 * p2.x - 3 * p3.x + p4.x) / 6.0;
            a[1] = (3 * p1.x - 6 * p2.x + 3 * p3.x) / 6.0;
            a[2] = (-3 * p1.x + 3 * p3.x) / 6.0;
            a[3] = (p1.x + 4 * p2.x + p3.x) / 6.0;
            b[0] = (-p1.y + 3 * p2.y - 3 * p3.y + p4.y) / 6.0;
            b[1] = (3 * p1.y - 6 * p2.y + 3 * p3.y) / 6.0;
            b[2] = (-3 * p1.y + 3 * p3.y) / 6.0;
            b[3] = (p1.y + 4 * p2.y + p3.y) / 6.0;
            splinex[0] = a[3];
            spliney[0] = b[3];
            int i;
            for (i = 1; i <= divisions - 1; i++)
            {
                float t = System.Convert.ToSingle(i) / System.Convert.ToSingle(divisions);
                splinex[i] = (a[2] + t * (a[1] + t * a[0])) * t + a[3];
                spliney[i] = (b[2] + t * (b[1] + t * b[0])) * t + b[3];
            }
        }
    }
}




