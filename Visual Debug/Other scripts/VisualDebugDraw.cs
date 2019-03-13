using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using Maumer.Internal;

namespace Maumer
{
    public static partial class VisualDebug
    {

        /*
         * Draw points 
         */
        
		[Conditional(runningInUnityEditor)]
		public static void DrawPoint(Vector3 position, float radius, bool wireframe = false)
		{
			DrawPoints(new Vector3[] { position }, radius, wireframe);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawPoints(IEnumerable<Vector3> points, float radius, bool wireframe = false)
		{
            AddArtistToCurrentFrame(new SphereArtist(points, radius, wireframe));
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawPoints(IEnumerable<Vector2> points, float radius, bool wireframe = false)
		{
			DrawPoints(EnumerableVector2ToVector3(points), radius, wireframe);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawPoints(float radius, bool wireframe, params Vector2[] points)
		{
			DrawPoints(EnumerableVector2ToVector3(points), radius, wireframe);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawPoints(float radius, bool wireframe, params Vector3[] points)
		{
			DrawPoints(points, radius, wireframe);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawPointWithLabel(Vector3 position, float radius, string text, int fontSize, bool wireframe = false)
		{
            // bool dontShowInBackground = m_debugDataDict.dontShowNextElementWhenFrameIsInBackground;
			DrawPoint(position, radius, wireframe);
            // m_debugDataDict.dontShowNextElementWhenFrameIsInBackground = dontShowInBackground;

            // TODO: Add font size option
            DrawTextWithHeightOffset(position + Vector3.up * radius, text, DebugData.defaultFontSize, true, 1);
            // DrawTextWithHeightOffset(position + Vector3.up * radius, text, m_debugDataDict.currentFontSize, true,1);
		}

        
		[Conditional(runningInUnityEditor)]
		public static void DrawPointWithLabel(Vector3 position, float radius, string text, bool wireframe = false)
		{
			// TODO: Add font size option
            DrawPointWithLabel(position, radius, text, DebugData.defaultFontSize, wireframe);
            // DrawPointWithLabel(position, radius, text, m_debugDataDict.currentFontSize, wireframe);
		}


		/*
         * Draw Lines
         */

		[Conditional(runningInUnityEditor)]
		[System.Obsolete("Use DrawLine instead")]
		public static void DrawLineSegment(Vector3 lineStart, Vector3 lineEnd)
		{
            AddArtistToCurrentFrame(new LinesArtist(new Vector3[] { lineStart, lineEnd }));
		}

		[Conditional(runningInUnityEditor)]
        [System.Obsolete("Use DrawLine instead")]
        public static void DrawLineSegmentWithLabel(Vector3 lineStart, Vector3 lineEnd, string text)
		{
            // bool dontShowInBackground = m_debugDataDict.dontShowNextElementWhenFrameIsInBackground;
            Vector3 textCentre = (lineStart + lineEnd) / 2f;
            DrawText(textCentre, text, true);
            // m_debugDataDict.dontShowNextElementWhenFrameIsInBackground = dontShowInBackground;
			AddArtistToCurrentFrame(new LinesArtist(new Vector3[] { lineStart, lineEnd }));
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawLineWithLabel(Vector3 from, Vector3 to, string text)
		{
            // bool dontShowInBackground = m_debugDataDict.dontShowNextElementWhenFrameIsInBackground;
            Vector3 textCentre = (from + to) / 2f;
            DrawText(textCentre, text, true);
            // m_debugDataDict.dontShowNextElementWhenFrameIsInBackground = dontShowInBackground;
            AddArtistToCurrentFrame(new LineArtist(from, to));
		}

		[Conditional(runningInUnityEditor)]
        public static void DrawLine(Vector3 from, Vector3 to)
        {
            AddArtistToCurrentFrame(new LineArtist(from, to));
        }

        [Conditional(runningInUnityEditor)]
        public static void DrawLine(Vector3 from, Vector3 to, string text)
		{
            // bool dontShowInBackground = m_debugDataDict.dontShowNextElementWhenFrameIsInBackground;
            Vector3 textCentre = (from + to) / 2f;
            DrawText(textCentre, text, true);
            // m_debugDataDict.dontShowNextElementWhenFrameIsInBackground = dontShowInBackground;
			AddArtistToCurrentFrame(new LineArtist(from, to));
            // AddArtistToCurrentFrame(new LinesArtist(new Vector3[] { lineStart, lineEnd }));
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawLineSegments(IEnumerable<Vector3> lineSegments)
		{
			AddArtistToCurrentFrame(new LinesArtist(lineSegments));
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawLineSegments(IEnumerable<Vector2> lineSegments)
		{
			DrawLineSegments(lineSegments.Select(v => new Vector3(v.x, v.y, 0)));
		}


		[Conditional(runningInUnityEditor)]
		public static void DrawLines(IEnumerable<Vector3> points, bool joinFirstAndLast = false)
		{
			Vector3[] pointsArray = points.ToArray();
			List<Vector3> lineSegments = new List<Vector3>();
			for (int i = 0; i < pointsArray.Length - 1; i++)
			{
				lineSegments.Add(pointsArray[i]);
				lineSegments.Add(pointsArray[i + 1]);
			}
			if (joinFirstAndLast)
			{
				lineSegments.Add(pointsArray[pointsArray.Length - 1]);
				lineSegments.Add(pointsArray[0]);
			}
			DrawLineSegments(lineSegments);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawLines(IEnumerable<Vector2> points, bool joinFirstAndLast = false)
		{
			DrawLines(EnumerableVector2ToVector3(points), joinFirstAndLast);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawLines(bool joinFirstAndLast, params Vector3[] points)
		{
			DrawLines(points, joinFirstAndLast);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawLines(bool joinFirstAndLast, params Vector2[] points)
		{
			DrawLines(EnumerableVector2ToVector3(points), joinFirstAndLast);
		}

		/*
         * Labels
         */

		[Conditional(runningInUnityEditor)]
		public static void DrawTextWithHeightOffset(Vector3 position, string text, int fontSize, bool centreAlign, float heightOffset)
		{
            AddArtistToCurrentFrame(new LabelArist(position, text, centreAlign, fontSize,heightOffset));
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawText(Vector3 position, string text, int fontSize, bool centreAlign = true)
		{
            DrawTextWithHeightOffset(position, text, fontSize, centreAlign,0);
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawText(Vector3 position, string text, bool centreAlign = true)
		{
			// TODO: add font size support
            // DrawText(position, text, m_debugDataDict.currentFontSize, centreAlign);
            DrawText(position, text, DebugData.defaultFontSize, centreAlign);
        }

		/*
         * Misc
         */

		[Conditional(runningInUnityEditor)]
		public static void DrawCube(Vector3 centre, float size)
		{
			AddArtistToCurrentFrame(new CubeArtist(centre, size));
		}

		[Conditional(runningInUnityEditor)]
		public static void DrawWireCube(Vector3 center, Vector3 size)
		{
			AddArtistToCurrentFrame(new Maumer.Internal.WireCubeArtist(center, size));
		}
	}
}
