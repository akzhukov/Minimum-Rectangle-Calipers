using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calipers
{
    static public class Data
    {
        static public List<Point2D> ReadData(string fileName)
        {
            try
            {
                string curdir = System.Environment.CurrentDirectory;
                var points = new List<Point2D>();
                string path = curdir + "\\" + fileName;
                if (File.Exists(path))
                {
                    using (StreamReader input = new StreamReader(fileName))
                    {
                        string[] pointsStr = File.ReadAllLines(path);
                        foreach (var point in pointsStr)
                        {
                            points.Add(new Point2D(point.Split(";")));
                        }
                    }

                }
                return points;
            }
            catch (Exception)
            {
                return null;
            }
        }
        static public void WriteData(string fileName, List<Point2D> points, List<int> indices, int supVecInd)
        {
            string curdir = Environment.CurrentDirectory;
            using (StreamWriter file = new StreamWriter(curdir + "\\" + fileName))
            {
                for(int i = supVecInd; i < indices.Count; i++)
                {
                    if (i == supVecInd)
                    {
                        int ind = indices[i] - 1;
                        if (ind == -1)
                            ind = points.Count - 1;
                        file.Write("{0};{1} ", points[ind].X, points[ind].Y);
                    }
                    else
                        file.Write("{0};{1} ", points[indices[i]].X, points[indices[i]].Y);
                }
                for(int i = 0; i < supVecInd; i++)
                {
                    file.Write("{0};{1} ", points[indices[i]].X, points[indices[i]].Y);
                }
            }

        }

    }
}
