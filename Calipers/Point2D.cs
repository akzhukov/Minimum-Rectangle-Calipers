using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Calipers
{
    public class Point2D
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }
        public Point2D(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public Point2D(string[] coords)
        {
            X = BigInteger.Parse(coords[0]);
            Y = BigInteger.Parse(coords[1]);
        }

        public static Point2D operator -(Point2D a) => new Point2D(-a.X, -a.Y);

        public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);

        public static Point2D operator -(Point2D a, Point2D b) => a + (-b);

        public static BigInteger operator *(Point2D a, Point2D b) => a.X * b.X + a.Y * b.Y;

        public Point2D Clockwise90()
        {
            BigInteger tmp;
            tmp = X;
            X = Y;
            Y = -tmp;
            return this;
        }

        public Point2D Clockwise270()
        {
            BigInteger tmp;
            tmp = X;
            X = -Y;
            Y = tmp;
            return this;
        }

        public BigInteger VecLenSqr()
        {
            return X * X + Y * Y;
        }

        public static BigInteger Def(Point2D p1, Point2D p2)
        {
            return p1.X * p2.Y - p2.X * p1.Y;
        }

        public static BigInteger CheckTriangleSign(Point2D p1, Point2D p2, Point2D p3)
        {
            BigInteger S = p1.X * p2.Y + p3.X * p1.Y + p2.X * p3.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
            //Console.WriteLine(S);
            return S == 0 ? 0 : S > 0 ? 1 : -1;
        }

        public static int CheckVectorsSign(Point2D vec1, Point2D vec2)
        {
            BigInteger S = vec1.X * vec2.Y - vec2.X * vec1.Y;
            return S == 0 ? 0 : S > 0 ? 1 : -1;
        }
    }

    public class AngleComparer : IComparer<Point2D>
    {
        public int Compare(Point2D vec1, Point2D vec2)
        {
            return Point2D.CheckVectorsSign(vec1, vec2);
        }
    }
}
