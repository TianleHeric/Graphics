using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicProject
{
    public class FillFace3D
    {
        public List<point3> pointSets3D;
        public List<Point> pointSets2D;
        public double depth;
        public Color faceColor;
        public FillFace3D()
        {
            pointSets3D = new List<point3>();
            pointSets2D = new List<Point>();
        }

        public FillFace3D(List<point3> pointsets3D, List<Point> pointsets2D)
        {
            pointSets3D = pointsets3D;
            pointSets2D = pointsets2D;
        }

        public void calcuDepth()
        {
            if (pointSets3D.Count == 4)
            {
                double sum = 0;
                foreach (point3 p3 in pointSets3D)
                {
                    sum += p3.z;
                }
                depth = sum / 4;
            }
            else
            {
                MessageBox.Show("some errors happened!!");
            }
        }
        public void set3Dpoints(List<point3> pointsets3D)
        {
            pointSets3D = pointsets3D;
        }
        public void set2DPoints(List<Point> pointsets2D)
        {
            pointSets2D = pointsets2D;
        }


    }

    public class point3
    {
        public double x;
        public double y;
        public double z;
        public double w;
        public point3()
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
            w = 1.0;
        }
        public point3(double x0, double y0, double z0)
        {
            x = x0;
            y = y0;
            z = z0;
            w = 1.0;
        }
    }

    public class face              //正方体的面
    {
        public int m_enum;    //面的边数
        public int[] p;     //顶点号，立方体，放顶点

        public face()
        {
        }

        public face(int num)
        {
            m_enum = num;
            p = new int[m_enum];
        }
        public void setedge(int i, int edge)
        {
            p[i] = edge;
        }
    }

    public class trans3
    {
        public double[,] transM = new double[4, 4];
        public point3[] pt;
        int m_num;

        public trans3()
        {

        }

        public trans3(point3[] p, int n)
        {
            m_num = n;
            pt = new point3[m_num];
            for (int i = 0; i < m_num; i++)
                pt[i] = p[i];
        }

        public void identityM()
        {
            transM[0, 0] = 1.0; transM[0, 1] = 0.0; transM[0, 2] = 0.0; transM[0, 3] = 0.0;
            transM[1, 0] = 0.0; transM[1, 1] = 1.0; transM[1, 2] = 0.0; transM[1, 3] = 0.0;
            transM[2, 0] = 0.0; transM[2, 1] = 0.0; transM[2, 2] = 1.0; transM[2, 3] = 0.0;
            transM[3, 0] = 0.0; transM[3, 1] = 0.0; transM[3, 2] = 0.0; transM[3, 3] = 1.0;
        }

        public void tranM(double tx, double ty, double tz)
        {
            identityM();
            transM[0, 3] = tx;
            transM[1, 3] = ty;
            transM[2, 3] = tz;
            multiM();

        }
        public void scaleM(double sx, double sy, double sz)
        {
            identityM();
            transM[0, 0] = sx;
            transM[1, 1] = sy;
            transM[2, 2] = sz;
            multiM();

        }

        public void scaleM(double sx, double sy, double sz, point3 p)
        {
            tranM(-p.x, -p.y, -p.z);
            scaleM(sx, sy, sz);
            tranM(p.x, p.y, p.z);

        }

        public void rotateMX(double angle)
        {
            identityM();
            double rad = angle * Math.PI / 180.0;
            transM[1, 1] = Math.Cos(rad); transM[1, 2] = Math.Sin(rad);
            transM[2, 1] = -Math.Sin(rad); transM[2, 2] = Math.Cos(rad);
            multiM();
        }

        public void rotateMX(double angle, point3 p)
        {
            tranM(-p.x, -p.y, -p.z);
            rotateMX(angle);
            tranM(p.x, p.y, p.z);
        }

        public void rotateMY(double angle)
        {
            identityM();
            double rad = angle * Math.PI / 180;
            transM[0, 0] = Math.Cos(rad); transM[0, 2] = Math.Sin(rad);
            transM[2, 0] = -Math.Sin(rad); transM[2, 2] = Math.Cos(rad);
            multiM();
        }

        public void rotateMY(double angle, point3 p)
        {
            tranM(-p.x, -p.y, -p.z);
            rotateMY(angle);
            tranM(p.x, p.y, p.z);
        }
        public void rotateMZ(double angle)
        {
            identityM();
            double rad = angle * Math.PI / 180;
            transM[0, 0] = Math.Cos(rad); transM[0, 1] = -Math.Sin(rad);
            transM[1, 0] = Math.Sin(rad); transM[1, 1] = Math.Cos(rad);
            multiM();
        }

        public void rotateMZ(double angle, point3 p)
        {
            tranM(-p.x, -p.y, -p.z);
            rotateMZ(angle);
            tranM(p.x, p.y, p.z);
        }
        public void multiM()
        {
            point3[] pt2 = new point3[m_num];
            for (int i = 0; i < m_num; i++)
            {
                pt2[i] = new point3(pt[i].x, pt[i].y, pt[i].z);

            }
            for (int j = 0; j < m_num; j++)
            {

                pt[j].x = pt2[j].x * transM[0, 0] + pt2[j].y * transM[0, 1] + pt2[j].z * transM[0, 2] + pt2[j].w * transM[0, 3];
                pt[j].y = pt2[j].x * transM[1, 0] + pt2[j].y * transM[1, 1] + pt2[j].z * transM[1, 2] + pt2[j].w * transM[1, 3];
                pt[j].z = pt2[j].x * transM[2, 0] + pt2[j].y * transM[2, 1] + pt2[j].z * transM[2, 2] + pt2[j].w * transM[2, 3];
                pt[j].w = pt2[j].x * transM[3, 0] + pt2[j].y * transM[3, 1] + pt2[j].z * transM[3, 2] + pt2[j].w * transM[3, 3];
            }
        }
    }
}
