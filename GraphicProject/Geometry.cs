using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicProject
{
    public class EdgeTable
    {
        public int Ymax;
        public int Ymin;
        public int Xmax;
        public int Xmin;
        public int X_ymin;
        public double slope;
        public EdgeTable(Point p1,Point p2)
        {
            if(p1.X>=p2.X)
            {
                Xmax = p1.X;
                Xmin = p2.X;
            }
            else
            {
                Xmax = p2.X;
                Xmin = p1.X;
            }
            if(p1.Y>=p2.Y)
            {
                Ymax = p1.Y;
                Ymin = p2.Y;
                X_ymin = p2.X;
            }
            else
            {
                Ymax = p2.Y;
                Ymin = p1.Y;
                X_ymin = p1.X;
            }
            slope = ((double)(p2.Y - p1.Y)) / ((double)(p2.X - p1.X));
        }
           

    }

    
    public static class Geometry
    {
        public static List<Point> polygonPointSets = new List<Point>();

        public static void fillPolygon(List<Point> pointSets,Color myColor,Bitmap mainBitmap)
        {
            List<EdgeTable> edgeTableSets = new List<EdgeTable>();
            if(pointSets.Count>=2)
            {
                for(int i=0;i<pointSets.Count;i++)
                {
                    try
                    {
                        edgeTableSets.Add(new EdgeTable(pointSets[i], pointSets[i + 1]));
                    }
                    catch(Exception e)
                    {
                        edgeTableSets.Add(new EdgeTable(pointSets[i], pointSets[0]));
                    }
                }
            }
            edgeTableSets = getOrdered(edgeTableSets);
            int Ytop = edgeTableSets[0].Ymax;
            int Ybottom = edgeTableSets[edgeTableSets.Count - 1].Ymin;
            foreach (EdgeTable eg in edgeTableSets)
            {
                if(eg.Ymin<Ybottom)
                {
                    Ybottom = eg.Ymin;
                }
            }

            int tempY = Ybottom;
            while(tempY<Ytop)
            {
                List<Point> pSets = new List<Point>();
                foreach(EdgeTable eg in edgeTableSets)
                {
                    if((tempY>=eg.Ymin)&&(tempY<eg.Ymax))
                    {
                        pSets.Add(new Point((int)((tempY - eg.Ymin) / eg.slope) + eg.X_ymin, tempY));
                    }
                }
                if(pSets.Count>=2)
                    Geometry.drawLine(pSets[0], pSets[1], myColor, mainBitmap);
                tempY++;
            }          
        }


        public static List<EdgeTable> getOrdered(List<EdgeTable> edgeTable)
        {
            List<EdgeTable> ordered = new List<EdgeTable>();
            EdgeTable maxEdge = edgeTable[0];
            //foreach (Edge eg in edgeTable)
            int c = edgeTable.Count;
            for (int a = 0; a < c; a++)
            {
                //foreach (Edge e in edgeTable)
                for (int w = 0; w < edgeTable.Count; w++)
                {
                    if (edgeTable[w].Ymax > maxEdge.Ymax)
                    {

                        maxEdge = edgeTable[w];
                    }

                }
                ordered.Add(maxEdge);
                edgeTable.Remove(maxEdge);
                if (edgeTable.Count > 0)
                {
                    maxEdge = edgeTable[0];
                }
            }

            return ordered;

        }

        public static void drawLine(Point startPoint, Point endPoint,Color myColor,Bitmap mainBitmap)
        {
            try
            {
                int x0 = startPoint.X, y0 = startPoint.Y, x1 = endPoint.X, y1 = endPoint.Y;

                int dx = Math.Abs(x1 - x0), dy = Math.Abs(y1 - y0);
                double slope = (double)(y1 - y0) / (double)(x1 - x0);
                int p = 2 * dy - dx, doubleDy = 2 * dy, doubleDyMinusDx = 2 * (dy - dx), x, y;
                int doubleDx = 2 * dx, doubleDxMinusDy = 2 * (dx - dy);

                if (slope >= 0 && slope < 1)
                {
                    if (x0 > x1)
                    {
                        x = x1;
                        y = y1;
                        x1 = x0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    //useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);

                    while (x < x1)
                    {
                        x++;
                        if (p < 0)
                        {
                            p += doubleDy;
                        }
                        else
                        {
                            y++;
                            p += doubleDyMinusDx;
                        }
                        //useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
                if (slope >= -1 && slope < 0)
                {
                    if (x0 > x1)
                    {
                        x = x1;
                        y = y1;
                        x1 = x0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    // useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);
                    while (x < x1)
                    {
                        x++;
                        if (p < 0)
                        {
                            p += doubleDy;

                        }
                        else
                        {
                            y--;
                            p += doubleDyMinusDx;
                        }
                        // useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
                if (slope <= -1)
                {

                    if (y0 < y1)
                    {
                        x = x1;
                        y = y1;
                        y1 = y0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    //useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);

                    while (y > y1)
                    {
                        y--;
                        if (p < 0)
                        {
                            p += doubleDx;
                        }
                        else
                        {
                            x++;
                            p += doubleDxMinusDy;
                        }
                        //useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
                if (slope > 1)
                {

                    if (y0 > y1)
                    {
                        x = x1;
                        y = y1;
                        y1 = y0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    //useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);

                    while (y < y1)
                    {
                        y++;
                        if (p < 0)
                        {
                            p += doubleDx;
                        }
                        else
                        {
                            x++;
                            p += doubleDxMinusDy;
                        }
                        //useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
            }
            catch
            {

            }
        }

        public static void bresenhamLine(Point startPoint, Point endPoint,Color myColor,Bitmap mainBitmap)
        {
            try
            {
                int x0 = startPoint.X, y0 = startPoint.Y, x1 = endPoint.X, y1 = endPoint.Y;

                int dx = Math.Abs(x1 - x0), dy = Math.Abs(y1 - y0);
                double slope = (double)(y1 - y0) / (double)(x1 - x0);
                int p = 2 * dy - dx, doubleDy = 2 * dy, doubleDyMinusDx = 2 * (dy - dx), x, y;
                int doubleDx = 2 * dx, doubleDxMinusDy = 2 * (dx - dy);

                if (slope >= 0 && slope < 1)
                {
                    if (x0 > x1)
                    {
                        x = x1;
                        y = y1;
                        x1 = x0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    //useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);

                    while (x < x1)
                    {
                        x++;
                        if (p < 0)
                        {
                            p += doubleDy;
                        }
                        else
                        {
                            y++;
                            p += doubleDyMinusDx;
                        }
                        //useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
                if (slope >= -1 && slope < 0)
                {
                    if (x0 > x1)
                    {
                        x = x1;
                        y = y1;
                        x1 = x0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    // useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);
                    while (x < x1)
                    {
                        x++;
                        if (p < 0)
                        {
                            p += doubleDy;

                        }
                        else
                        {
                            y--;
                            p += doubleDyMinusDx;
                        }
                        // useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
                if (slope <= -1)
                {

                    if (y0 < y1)
                    {
                        x = x1;
                        y = y1;
                        y1 = y0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    //useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);

                    while (y > y1)
                    {
                        y--;
                        if (p < 0)
                        {
                            p += doubleDx;
                        }
                        else
                        {
                            x++;
                            p += doubleDxMinusDy;
                        }
                        //useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
                if (slope > 1)
                {

                    if (y0 > y1)
                    {
                        x = x1;
                        y = y1;
                        y1 = y0;
                    }
                    else
                    {
                        x = x0;
                        y = y0;
                    }
                    //useApi.SetPixel(hDC, x, y, cc);
                    mainBitmap.SetPixel(x, y, myColor);

                    while (y < y1)
                    {
                        y++;
                        if (p < 0)
                        {
                            p += doubleDx;
                        }
                        else
                        {
                            x++;
                            p += doubleDxMinusDy;
                        }
                        //useApi.SetPixel(hDC, x, y, cc);
                        mainBitmap.SetPixel(x, y, myColor);
                    }
                }
            }
            catch
            {

            }
        }

        public static void ddaLine(Point startPoint, Point endPoint,Color myColor,Bitmap mainBitmap)
        {
            int x0 = startPoint.X, y0 = startPoint.Y, x1 = endPoint.X, y1 = endPoint.Y;
            //int cc = useApi.GetCustomColor(c);
            //useApi.SetPixel(hDC, x0, y0, cc);

            mainBitmap.SetPixel(x0, y0, myColor);
            int dx = x1 - x0, dy = y1 - y0, steps, num;
            double xIncrement, yIncrement, x = x0, y = y0;
            if (Math.Abs(dx) > Math.Abs(dy))
                steps = Math.Abs(dx);
            else
                steps = Math.Abs(dy);
            xIncrement = (double)dx / (double)steps;
            yIncrement = (double)dy / (double)steps;
            for (num = 0; num < steps; num++)
            {
                x += xIncrement;
                y += yIncrement;
                //useApi.SetPixel(hDC, (int)x, (int)y, cc);
                mainBitmap.SetPixel((int)x, (int)y, myColor);
            }
        }

        public static void potLine(Point startPoint, Point endPoint,Color myColor,Bitmap mainBitmap)
        {
            int x0 = startPoint.X, y0 = startPoint.Y, x1 = endPoint.X, y1 = endPoint.Y;
            int x = x0, y = y0;
            int f = 0;   //the judger 
            int num = Math.Abs((x1 - x0)) + Math.Abs((y1 - y0));
            mainBitmap.SetPixel(x0, y0, myColor);
            if (x0 <= x1 && y0 <= y1)
            {
                while (num > 0)
                {
                    if (f >= 0)
                    {
                        x += 1;
                        f = f - (y1 - y0);
                    }
                    else
                    {
                        y += 1;
                        f = f + (x1 - x0);
                    }
                    mainBitmap.SetPixel(x, y, myColor);
                    num--;
                }
            }
            else if (x0 <= x1 && y0 > y1)
            {
                while (num > 0)
                {
                    if (f >= 0)
                    {
                        y -= 1;
                        f = f - Math.Abs(x1 - x0);
                    }
                    else
                    {
                        x += 1;
                        f = f + Math.Abs(y1 - y0);
                    }
                    mainBitmap.SetPixel(x, y, myColor);
                    num--;
                }
            }

            else if (x0 > x1 && y0 > y1)//third
            {
                while (num > 0)
                {
                    if (f >= 0)
                    {
                        x -= 1;
                        f = f - Math.Abs(y1 - y0);
                    }
                    else
                    {
                        y -= 1;
                        f = f + Math.Abs(x1 - x0);
                    }
                    mainBitmap.SetPixel(x, y, myColor);
                    num--;
                }
            }
            else if (x0 >= x1 && y0 < y1)
            {
                while (num > 0)
                {
                    if (f >= 0)
                    {
                        y += 1;
                        f = f - Math.Abs(x1 - x0);
                    }
                    else
                    {
                        x -= 1;
                        f = f + Math.Abs(y1 - y0);
                    }


                    mainBitmap.SetPixel(x, y, myColor);
                    num--;
                }
            }
            while (num > 0)
            {
                if (f >= 0)
                {
                    x += 1;
                    f = f - Math.Abs(y1 - y0);
                }
                else
                {
                    y -= 1;
                    f = f + Math.Abs(x1 - x0);
                }
                mainBitmap.SetPixel(x, y, myColor);
                num--;
            }
        }
        public static void drawCircle(Point startPoint,Point endPoint,Color myColor,Bitmap mainBitmap)
        {
            int radius = (int)Math.Sqrt((endPoint.X - startPoint.X) * (endPoint.X - startPoint.X) + (endPoint.Y - startPoint.Y) * (endPoint.Y - startPoint.Y));
            int x = 0;
            int y = radius;
            double xMid;
            double yMid;
            int xMax = (int)(((double)radius) / Math.Sqrt(2.0));

            while (x <= xMax)
            {            
               try
                {
                    mainBitmap.SetPixel(startPoint.X + x, startPoint.Y + y, myColor);
                    mainBitmap.SetPixel(startPoint.X + x, startPoint.Y - y, myColor);
                    mainBitmap.SetPixel(startPoint.X + y, startPoint.Y - x, myColor);
                    mainBitmap.SetPixel(startPoint.X + y, startPoint.Y + x, myColor);
                    mainBitmap.SetPixel(startPoint.X - x, startPoint.Y + y, myColor);
                    mainBitmap.SetPixel(startPoint.X - x, startPoint.Y - y, myColor);
                    mainBitmap.SetPixel(startPoint.X - y, startPoint.Y + x, myColor);
                    mainBitmap.SetPixel(startPoint.X - y, startPoint.Y - x, myColor);
                }
                catch(Exception e)
                {
                }
                xMid = x;
                yMid = ((double)((2 * y - 1))) / 2.0;
                if (Math.Sqrt((xMid) * (xMid) + (yMid) * (yMid)) < radius)
                {
                }
                else
                {
                    y--;
                }
                x++;
            }
        }



        public static void drawEllipse(Point startPoint, Point endPoint, Color myColor, Bitmap mainBitmap)
        {
            try
            {
                int a = Math.Abs(endPoint.X - startPoint.X), b = Math.Abs(endPoint.Y - startPoint.Y);
                int x = 0;
                int y = b;

                double midPointX = x + 1;
                double midPointY = y - 0.5;

                int tempPoint = (int)(((double)a * (double)a) / (Math.Sqrt((double)a * (double)a + (double)b * (double)b)));
                while (x <= tempPoint)
                {
                    mainBitmap.SetPixel(startPoint.X + x, startPoint.Y + y, myColor);
                    mainBitmap.SetPixel(startPoint.X - x, startPoint.Y + y, myColor);
                    mainBitmap.SetPixel(startPoint.X + x, startPoint.Y - y, myColor);
                    mainBitmap.SetPixel(startPoint.X - x, startPoint.Y - y, myColor);


                    if ((b * b * midPointX * midPointX + a * a * midPointY * midPointY - a * a * b * b) >= 0)
                    {
                        y--;
                    }
                    x++;
                    midPointX = x + 1;
                    midPointY = y - 0.5;
                }
                midPointX = x + 0.5;
                midPointY = y - 1;
                while (y >= 0)
                {
                    mainBitmap.SetPixel(startPoint.X + x, startPoint.Y + y, myColor);
                    mainBitmap.SetPixel(startPoint.X - x, startPoint.Y + y, myColor);
                    mainBitmap.SetPixel(startPoint.X + x, startPoint.Y - y, myColor);
                    mainBitmap.SetPixel(startPoint.X - x, startPoint.Y - y, myColor);
                    if ((b * b * midPointX * midPointX + a * a * midPointY * midPointY - a * a * b * b) < 0)
                    {
                        x++;
                    }
                    y--;
                    midPointX = x + 0.5;
                    midPointY = y - 1;
                }
            }
            catch (Exception e)
            { }
        }

        public static void setPixel(Point p, Color myColor, Bitmap mainBitmap)
        {
            mainBitmap.SetPixel(p.X, p.Y, myColor);
        }

        public static Rectangle makeRectangleFromPoints(Point p1, Point p2)
        {
            int left, top, bottom, right;
            top = p1.Y <= p2.Y ? p1.Y : p2.Y;
            left = p1.X <= p2.X ? p1.X : p2.X;
            bottom = p1.Y > p2.Y ? p1.Y : p2.Y;
            right = p1.X > p2.X ? p1.X : p2.X;
            return (new Rectangle(left, top, right - left, bottom - top));
        }


    }
}
