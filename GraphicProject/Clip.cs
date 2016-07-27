using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicProject
{
    public static class Clip
    {
        public static int left = 200, right = 300, top = 200, buttom = 300;
        public static int CodeLeft = 1;
        public static int CodeRight = 2;
        public static int CodeButtom = 4;
        public static int CodeTop = 8;
        public static void CohenSutherland(int P1x, int P1y, int P2x, int P2y,Color myColor,Bitmap mainBitmap)
        {
            int C1 = Code(P1x, P1y), C2 = Code(P2x, P2y);
            int C;
            int Px = 0, Py = 0;//记录交点  
            while (C1 != 0 || C2 != 0)//两个点（P1x,P1y）,（P2x,P2y）不都在矩形框内；都在内部就画出线段  
            {
                if ((C1 & C2) != 0)   //两个点在矩形框的同一外侧 → 不可见  
                {
                    P1x = 0;
                    P1y = 0;
                    P2x = 0;
                    P2y = 0;
                    break;
                }
                C = C1;
                if (C1 == 0)// 判断P1 P2谁在矩形框内（可能是P1，也可能是P2）  
                {
                    C = C2;
                }

                if ((C & CodeLeft) != 0)//用与判断的点在左侧   
                {
                    Px = left;
                    Py = P1y + (int)(Convert.ToDouble(P2y - P1y) / (P2x - P1x) * (left - P1x));
                }
                else if ((C & CodeRight) != 0)//用与判断的点在右侧   
                {
                    Px = right;
                    Py = P1y + (int)(Convert.ToDouble(P2y - P1y) / (P2x - P1x) * (right - P1x));
                }
                else if ((C & CodeTop) != 0)//用与判断的点在上方  
                {
                    Py = top;
                    Px = P1x + (int)(Convert.ToDouble(P2x - P1x) / (P2y - P1y) * (top - P1y));
                }
                else if ((C & CodeButtom) != 0)//用与判断的点在下方  
                {
                    Py = buttom;
                    Px = P1x + (int)(Convert.ToDouble(P2x - P1x) / (P2y - P1y) * (buttom - P1y));
                }

                if (C == C1) //上面判断使用的是哪个端点就替换该端点为新值  
                {
                    P1x = Px;
                    P1y = Py;
                    C1 = Code(P1x, P1y);
                }
                else
                {
                    P2x = Px;
                    P2y = Py;
                    C2 = Code(P2x, P2y);
                }
            }
            Geometry.potLine(new Point(P1x, P1y), new Point(P2x, P2y), Color.Black, mainBitmap);            
        }

        public static int Code(int x, int y) 
        {
            int c = 0;
            if (x < left)
            {
                c = c | CodeLeft;
            }
            if (x > right)
            {
                c = c | CodeRight;
            }
            if (y < top)
            {
                c = c | CodeTop;
            }
            if (y > buttom)
            {
                c = c | CodeButtom;
            }
            return c;
        }
    }
}


