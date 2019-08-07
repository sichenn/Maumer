using UnityEngine;
using Maumer;

namespace Maumer.Example
{
    public static class ExampleAlgorithm
    {
        // Returns an array containing the points nearest to one another out of the given set
        public static Vector3[] FindClosestPairOfPoints(Vector3[] points)
        {
            VisualDebug.BeginFrame("All points", keepInBackground: true);
            VisualDebug.DrawPoints(points, .1f);
            Vector3[] closestPointPair = new Vector3[2];
            float bestDst = float.MaxValue;

            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    float dst = Vector3.Distance(points[i], points[j]);
                    if (dst < bestDst)
                    {
                        bestDst = dst;
                        closestPointPair[0] = points[i];
                        closestPointPair[1] = points[j];
                    }
                    VisualDebug.BeginFrame("Compare dst", keepInBackground: true);
                    VisualDebug.SetColour(Colours.lightRed, Colours.veryDarkGrey);
                    VisualDebug.DrawPoint(points[i], .1f);
                    VisualDebug.DrawLine(points[i], points[j]);
                    VisualDebug.DontShowNextElementWhenFrameIsInBackground();
                    VisualDebug.SetColour(Colours.lightGreen);
                    VisualDebug.DrawLine(closestPointPair[0], closestPointPair[1], bestDst.ToString());
                }
            }

            VisualDebug.BeginFrame("Finished");
            VisualDebug.SetColour(Colours.lightGreen);
            VisualDebug.DrawPoints(closestPointPair, .15f);
            VisualDebug.DrawLine(closestPointPair[0], closestPointPair[1], bestDst.ToString());
            VisualDebug.Save();
            return closestPointPair;
        }
    }
}