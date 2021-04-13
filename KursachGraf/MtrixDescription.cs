using System;
using System.Collections.Generic;
using System.Drawing;

namespace KursachGraf
{
    class MatrixDescription
    {
        public class Points
        {
            public double x;
            public double y;
            public double z;
            public Points(){}
            public Points(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
        public class Polygon
        {
            public Points point_1;
            public Points point_2;
            public Points point_3;
            public Points point_4;
            public Polygon() { }
            public Polygon(Points point_1, Points point_2, Points point_3, Points point_4)
            {
                this.point_1 = point_1;
                this.point_2 = point_2;
                this.point_3 = point_3;
                this.point_4 = point_4;
            }

        }
        public static Points RotateX(Points points, double sinDeg, double cosDeg)
        {
            double _y = (points.y * cosDeg) + (points.z * sinDeg);
            double _z = (points.y * -sinDeg) + (points.z * cosDeg);

            return new Points(points.x, _y, _z);
        }
        public static Points RotateY(Points points, double sinDeg, double cosDeg)
        {
            double _x = (points.x * cosDeg) + (points.z * sinDeg);
            double _z = (points.x * -sinDeg) + (points.z * cosDeg);
            double _y = points.y; 
            return new Points(_x, _y, _z);
        }
        public static Points RotateZ(Points points, double sinDeg, double cosDeg)
        {
            double _x = (points.x * cosDeg) + (points.y * sinDeg);
            double _y = (points.x * -sinDeg) + (points.y * cosDeg);

            return new Points(_x, _y, points.z);
        }
        public static List<Points> RotateX(List<Points> points, double deg)
        {
            double radDeg = (Math.PI * deg) / 180.0;
            double cosDeg = Math.Cos(radDeg);
            double sinDeg = Math.Sin(radDeg);
            for (int i = 0; i < points.Count; i++)
                    points[i] = RotateX(points[i], sinDeg, cosDeg);
            points = SearchWH(points);
            return points;
        }

        public static List<Points> RotateY(List<Points> points, double deg)
        {
            double radDeg = (Math.PI * deg) / 180.0;
            double cosDeg = Math.Cos(radDeg);
            double sinDeg = Math.Sin(radDeg);
            for (int i = 0; i < points.Count; i++) 
                    points[i] = RotateY(points[i], sinDeg, cosDeg);
            points = SearchWH(points);
 
            return points;
        }
        public static List<Points> RotateZ(List<Points> points, double deg)
        {
            double radDeg = (Math.PI * deg) / 180.0;
            double cosDeg = Math.Cos(radDeg);
            double sinDeg = Math.Sin(radDeg);
            for (int i = 0; i < points.Count; i++)
                    points[i] = RotateZ(points[i], sinDeg, cosDeg);
            points = SearchWH(points);
            return points;
        }
        public static List<Points> SearchWH(List<Points> points)
        {
  
            int W = 1000000, H = 100000;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].x < W)
                    W = (int)points[i].x;
                if (points[i].y < H)
                    H = (int)points[i].y;
       
            }
            points = Out(points, 0, 0);
            return points;
        }
        public static List<Points> Out(List<Points> points, int x, int y)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].x += x;
                points[i].y += y;
            }

            return points;
        }

        public static Point[] Otrtransform(List<Polygon> coordList, double[,] arr, int i)
        {
            Point[] p = new Point[4];
            p[0].X = (int)coordList[(int)arr[i, 1]].point_1.x+300;
            p[0].Y = (int)coordList[(int)arr[i, 1]].point_1.y+300;

            p[1].X = (int)coordList[(int)arr[i, 1]].point_2.x+300;
            p[1].Y = (int)coordList[(int)arr[i, 1]].point_2.y + 300;

            p[2].X = (int)coordList[(int)arr[i, 1]].point_3.x + 300;
            p[2].Y = (int)coordList[(int)arr[i, 1]].point_3.y + 300;

            p[3].X = (int)coordList[(int)arr[i, 1]].point_4.x + 300;
            p[3].Y = (int)coordList[(int)arr[i, 1]].point_4.y + 300;

            return p;
        } 



    }
}
