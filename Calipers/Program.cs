using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;

namespace Calipers
{
    class Program
    {
        static List<Point2D> points = null;
        static int supVecInd;
        const string inputFile = "input.txt";
        const string outputFileArea = "outputArea.txt";
        const string outputFilePerimeter = "outputPerimeter.txt";
        const bool outputConsole = false;
        static void Main(string[] args)
        {
            List<int> answerArea = null;
            List<int> answerPerimeter = null;
            int answerAreaSupVecInd = 0;
            int answerPerimeterSupVecInd = 0;
            points = Data.ReadData(inputFile);

            if (points == null)
            {
                Console.WriteLine("Неверные входные данные");
                return;
            }
            if (!CheckConvex())
            {
                Console.WriteLine("Многоугольник не выпуклый");
                return;
            }
            var set = NextPointsSet().GetEnumerator();
            Fraction S = null, P = null;
            Fraction tmp;
            while (set.MoveNext())
            {
                List<int> pointsSet = set.Current;
                tmp = CalcPerimeter(pointsSet);
                if (P == null || Fraction.Compare(P, tmp) == 1)
                {
                    P = tmp;
                    answerPerimeter = pointsSet.ToList();
                    answerPerimeterSupVecInd = supVecInd;
                }
                tmp = CalcArea(pointsSet);
                if (S == null || Fraction.Compare(S, tmp) == 1)
                {
                    S = tmp;
                    answerArea = pointsSet.ToList();
                    answerAreaSupVecInd = supVecInd;
                }
            }

            if (outputConsole)
            {
                Console.Write("Min P = {0}/{1} with points set ", P.Numerator, P.Denominator);
                answerPerimeter.ForEach(Console.Write);
                Console.WriteLine();
                Console.Write("Min S = {0}/{1} with points set ", S.Numerator, S.Denominator);
                answerArea.ForEach(Console.Write);
                Console.WriteLine();
            }
            Data.WriteData(outputFileArea, points, answerArea, answerAreaSupVecInd);
            Data.WriteData(outputFilePerimeter, points, answerPerimeter, answerPerimeterSupVecInd);
        }


        static bool CheckConvex()
        {
            BigInteger signPrev = 0, signNext;
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (i != points.Count - 2)
                    signNext = Point2D.CheckTriangleSign(points[i], points[i + 1], points[i + 2]);
                else
                    signNext = Point2D.CheckTriangleSign(points[i], points[i + 1], points[0]);
                if (signNext == 0)
                    continue;
                if (signPrev != 0 && signPrev != signNext)
                    return false;
                signPrev = signNext;
            }
            if (signPrev == -1)
                points.Reverse();
            return true;
        }

        static int MinAngleIndex(List<Point2D> vectors)
        {
            List<Point2D> listCopy = vectors.ToList();
            listCopy.Sort(new AngleComparer());

            return vectors.IndexOf(listCopy[vectors.Count - 1]);
        }

        static List<Point2D> MoveVectors(List<int> indices)
        {
            var vectors = new List<Point2D>(4);
            vectors.Add(points[(indices[0] + 1) % points.Count] - points[indices[0]]);
            vectors.Add((points[(indices[1] + 1) % points.Count] - points[indices[1]]).Clockwise90());
            vectors.Add(-(points[(indices[2] + 1) % points.Count] - points[indices[2]]));
            vectors.Add((points[(indices[3] + 1) % points.Count] - points[indices[3]]).Clockwise270());
            return vectors;
        }

        static Fraction CalcArea(List<int> indices)
        {
            int prevSupInd = indices[supVecInd] - 1;
            if (prevSupInd == -1)
                prevSupInd = points.Count - 1;
            Point2D p1 = points[prevSupInd];
            Point2D p2 = points[indices[supVecInd]];
            Point2D p4 = points[indices[(supVecInd + 1) % indices.Count]];
            Point2D p3 = points[indices[(supVecInd + 2) % indices.Count]];
            Point2D p5 = points[indices[(supVecInd + 3) % indices.Count]];
            Point2D n = (p2 - p1).Clockwise270();
            var ans = new Fraction(((p3 - p1) * n) * ((p4 - p5) * (p2 - p1)), n.VecLenSqr());
            return ans;
        }

        //возвращает квадрат полупериметра
        static Fraction CalcPerimeter(List<int> indices)
        {
            int prevSupInd = indices[supVecInd] - 1;
            if (prevSupInd == -1)
                prevSupInd = points.Count - 1;
            Point2D p1 = points[prevSupInd];
            Point2D p2 = points[indices[supVecInd]];
            Point2D p4 = points[indices[(supVecInd + 1) % indices.Count]];
            Point2D p3 = points[indices[(supVecInd + 2) % indices.Count]];
            Point2D p5 = points[indices[(supVecInd + 3) % indices.Count]];
            Point2D n = (p2 - p1).Clockwise270();
            BigInteger numerator = (p3 - p1) * n + (p4 - p5) * (p2 - p1);
            var ans = new Fraction(numerator * numerator, n.VecLenSqr());
            return ans;
        }

        static IEnumerable<List<int>> NextPointsSet()
        {
            BigInteger numberIters = points.Count() + 1;
            BigInteger currentIter = 0;
            BigInteger minY = points[0].Y;
            BigInteger maxY = points[0].Y;
            BigInteger minX = points[0].X;
            BigInteger maxX = points[0].X;
            List<int> pointsSet = new List<int>(4);
            for (int i = 0; i < 4; i++)
                pointsSet.Add(0);
            for (int i = 0; i < points.Count; ++i)
            {
                if (points[i].Y <= minY)
                {
                    pointsSet[0] = i;
                    minY = points[i].Y;
                }
                if (points[i].X >= maxX)
                {
                    pointsSet[1] = i;
                    maxX = points[i].X;
                }
                if (points[i].Y >= maxY)
                {
                    pointsSet[2] = i;
                    maxY = points[i].Y;
                }
                if (points[i].X <= minX)
                {
                    pointsSet[3] = i;
                    minX = points[i].X;
                }

            }

            supVecInd = MinAngleIndex(MoveVectors(pointsSet));
            pointsSet[supVecInd]++;
            pointsSet[supVecInd] %= points.Count;
            yield return pointsSet;
            while (currentIter < numberIters)
            {
                currentIter++;
                supVecInd = MinAngleIndex(MoveVectors(pointsSet));
                pointsSet[supVecInd]++;
                pointsSet[supVecInd] %= points.Count;
                yield return pointsSet;
            }
        }
   }
}
