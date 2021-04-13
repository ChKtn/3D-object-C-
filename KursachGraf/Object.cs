using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace KursachGraf
{
    class Object : MatrixDescription
    {
        Pen pen = new Pen(Color.Black, 1);
        readonly List<Points> vertices = new List<Points>();
        readonly List<int[]> sides = new List<int[]>();
        System.IO.StreamReader file = new System.IO.StreamReader(@"D:\Vjt\FL\5sm\Graphic\Курсач\kb - Copy.txt");

        public double Speed { get; set; }
        public Object()
        {   
            SetVertices(file);
            SetSide(file);
        }
        private void  SetSide(System.IO.StreamReader file)
        {
            string[] separatingStrings = { " ", "//" };
            string line;
            int[] iter = new int[8];
            line = file.ReadLine();
            if (line == "") line += '7';
            while (line[0] != 'f') { line = file.ReadLine(); if (line == "") line += '7'; }
            while (line[0] == 'f')
            {
                string[] words = line.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < 9; i++)
                {
                    iter[i-1] = Int32.Parse(words[i]);
                    iter[i - 1] -= 1;
                }
                line = file.ReadLine();
                if (line == null) line += '7';
                sides.Add(new int[] { iter[0], iter[2], iter[4], iter[6]});
            }
        }
        private void SetVertices(System.IO.StreamReader file)
        {
            string line;
            line = file.ReadLine();
            while (line[0] != 'v') { 
                line = file.ReadLine();
                if (line == "") line += '7';
            }
            while (line[0] == 'v') {
                vertices.Add(SetVertex(line));
                line = file.ReadLine();
                if (line == "") line += '7';
            }
        }
        public Points SetVertex(string line)
        {
            double kf = 50.0;
            Points point = new Points();
            string[] words = line.Split(' ');
            point.x = double.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat)*kf;
            point.y = double.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat)*kf;
            point.z = double.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat)*kf;
            double radDeg = (Math.PI * 0) / 180.0;
            double cosDeg = Math.Cos(radDeg);
            double sinDeg = Math.Sin(radDeg);
              point = RotateZ(point, 1, 0);
             // point = RotateX(point, -1, 0);
            //point = RotateY(point, -1, 0);

            return point;
        }
        public Bitmap DrawingTor(int _width, int _height, int type, double deg)
        {
            Bitmap bitmap = new Bitmap(_width, _height);
            Graphics Graphic = Graphics.FromImage(bitmap);
            Graphic.SmoothingMode = SmoothingMode.AntiAlias;

            List<Points> points = new List<Points>();
            points = vertices;
            //points = RotateX(points, deg);
            points = RotateY(points, deg);
            //points = RotateZ(points, deg);

            List<Polygon> coordList = new List<Polygon>();
            for (int i = 0; i < sides.Count; i++)
                    coordList.Add(new Polygon(points[sides[i][0]], points[sides[i][1]], points[sides[i][2]], points[sides[i][3]]));

            double cur = 0;
            double[,] arr = new double[coordList.Count, 2];
            for (int i = 0; i < coordList.Count(); i++)
            {
                cur = (coordList[i].point_1.z + coordList[i].point_2.z + coordList[i].point_3.z + coordList[i].point_4.z) / 4;
                arr[i, 0] = cur;
                arr[i, 1] = i;
            }

            double[] NormAngle = new double[coordList.Count];
            for (int m = 0; m < coordList.Count; m++)
            {
                NormAngle[m] = CosNormal(coordList[m]);
            }

            SortZ(arr, 0, coordList.Count - 1);
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                Point[] p = Otrtransform(coordList, arr, i);
                if (type == 1)
                {
                    int _alpha = Math.Abs((int)(255 * (NormAngle[(int)arr[i, 1]])));
                    Color clr = Color.FromArgb(_alpha, 25, 100, 120);
                    SolidBrush dr = new SolidBrush(clr);
                    Graphic.FillPolygon(Brushes.Black, p);
                    Graphic.FillPolygon(dr, p);
                }
                else
                    Graphic.DrawPolygon(pen, p);
            }

            return bitmap;
        }
        private double CosNormal(Polygon p)
        {
            double cos = new double();
            double[] coord = new double[3];
            coord[0] = (p.point_2.y - p.point_1.y) * (p.point_3.z - p.point_1.z) - (p.point_2.z - p.point_1.z) * (p.point_3.y - p.point_1.y); 
            coord[1] = (p.point_2.z - p.point_1.z) * (p.point_3.x - p.point_1.x) - (p.point_2.x - p.point_1.x) * (p.point_3.z - p.point_1.z);  
            coord[2] = (p.point_2.x - p.point_1.x) * (p.point_3.y - p.point_1.y) - (p.point_2.y - p.point_1.y) * (p.point_3.x - p.point_1.x);  
            cos = coord[2] / (Math.Sqrt(Math.Pow(coord[0], 2) + Math.Pow(coord[1], 2) + Math.Pow(coord[2], 2)));
            return cos;
        }
        private void SortZ(double[,] _facets, int _in, int _out)
        {
            if (_in >= _out) return;
            int temp = Border(_facets, _in, _out);
            SortZ(_facets, _in, temp - 1);
            SortZ(_facets, temp + 1, _out);
        }
         private static int Border(double[,] arr, int first, int second)
        {
            int temp = first;
            for (int i = first; i <=second; i++)
            {
                if (arr[i, 0] <=arr[second, 0])
                {
                    double t =arr[temp, 0];
                   arr[temp, 0] =arr[i, 0];
                   arr[i, 0] = t;
                    t =arr[temp, 1];
                   arr[temp, 1] =arr[i, 1];
                   arr[i, 1] = t;
                    temp++;
                }
            }
            return temp - 1;
        }
    }
}
