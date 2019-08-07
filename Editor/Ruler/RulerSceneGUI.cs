using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Maumer
{
    /// <summary>
    /// This tool is made for debugging in 2D circular worlds
    /// </summary>
    [InitializeOnLoad]
    public static class RulerSceneGUI
    {
        public static bool show = false;
        public static float startAngle = 0;
        public static int rulerDivisions = 0;
        private static Vector3 m_mousePosInWorld;
        private static Camera m_sceneViewCamera;
        private static Color k_lineColor = new Color(.22f, .22f, .22f);


        /// <summary>
        /// Called at startup
        /// </summary>
        static RulerSceneGUI()
        {
            EditorApplication.update += Update;
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        static void Update()
        {
            //anything you want to do every frame..
            m_sceneViewCamera = SceneView.lastActiveSceneView.camera;
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (m_sceneViewCamera == null)
            {
                return;
            }
            if (show)
            {

                if (m_sceneViewCamera.orthographic)
                {
                    DisplayRulerBars();
                }
            }
        }

        private static void DisplayRulerBars()
        {
            int size = 16;
            int height = Screen.height - size * 2 - 5;
            Handles.BeginGUI();
            // Draw top and side ruler bar
            GUI.Box(new Rect(0, 0, size, size), "", EditorStyles.toolbar);
            // horizontal Ruler
            GUI.Box(new Rect(size, 0, Screen.width - size, size), "", EditorStyles.textArea);
            // vertical Ruler
            GUI.Box(new Rect(0, size, size, height), "", EditorStyles.textArea);

            // 37 is a magic number that cuts off the erroneous "extra" height from Screen.height 
            int unitsPerCell = GetUnitsPerColumn(m_sceneViewCamera.orthographicSize * 2);
            int numSubcells = GetFirstNDigits(unitsPerCell, 2);
            if (numSubcells > 10)
            {
                numSubcells = 10;
            }

            float sizePerUnit = height / (m_sceneViewCamera.orthographicSize * 2);
            Vector2 lineOrigin = new Vector2(size, 0);
            Vector2 offset = m_sceneViewCamera.ScreenToWorldPoint(new Vector2(size, Screen.height - size - 37));
            lineOrigin.x += ((int)(offset.x / unitsPerCell) * unitsPerCell - offset.x - unitsPerCell) * sizePerUnit;


            Handles.color = k_lineColor;
            Vector2 linePos = lineOrigin;

            // draw the horizontal ruler
            int counter = (int)(offset.x / unitsPerCell) - 1;
            while (lineOrigin.x < Screen.width)
            {
                linePos.x = Mathf.Max(lineOrigin.x, size);
                Handles.DrawLine(linePos, linePos + Vector2.up * size);
                GUI.Box(new Rect(linePos.x, linePos.y,
                    lineOrigin.x < 0 ? (unitsPerCell * sizePerUnit + lineOrigin.x - size) : unitsPerCell * sizePerUnit, size),
                    (counter * unitsPerCell).ToString(),
                    EditorStyles.label);
                lineOrigin.x += 1f / numSubcells * sizePerUnit * unitsPerCell;
                for (int i = 1; i < numSubcells; i++)
                {
                    linePos.x = Mathf.Max(lineOrigin.x, size);
                    Handles.DrawLine(linePos + Vector2.up * size, linePos + Vector2.up * .75f * size);
                    lineOrigin.x += 1f / numSubcells * sizePerUnit * unitsPerCell;
                }
                counter++;
            }

            lineOrigin.x = 0;
            lineOrigin.y = size;
            lineOrigin.y -= ((int)(offset.y / unitsPerCell) * unitsPerCell - offset.y + unitsPerCell) * sizePerUnit;
            linePos = lineOrigin;
            counter = (int)(offset.y / unitsPerCell) + 1;
            // draw vertical ruler
            while (lineOrigin.y < height)
            {
                linePos.y = Mathf.Max(lineOrigin.y, size);
                Handles.DrawLine(linePos, linePos + Vector2.right * size);
                GUI.Box(new Rect(linePos.x, linePos.y,
                    size, lineOrigin.y < 0 ? (unitsPerCell * sizePerUnit + lineOrigin.y - size) : unitsPerCell * sizePerUnit),
                    (counter * unitsPerCell).ToString(), EditorStyles.wordWrappedLabel);
                lineOrigin.y += 1f / numSubcells * sizePerUnit * unitsPerCell;
                for (int i = 1; i < numSubcells; i++)
                {
                    linePos.y = Mathf.Max(lineOrigin.y, size);
                    Handles.DrawLine(linePos + Vector2.right * size, linePos + Vector2.right * .75f * size);
                    lineOrigin.y += 1f / numSubcells * sizePerUnit * unitsPerCell;
                }
                counter--;
            }

            Handles.EndGUI();
        }

        /// <summary>
        /// Choose a intuitive ruler column size from given screen size
        /// </summary>
        /// <param name="viewportWorldHeight">the width of the viewport in world coordinate (most likely the camera ortho size)</param>
        /// <param name="worldSizeTolerance"></param>
        /// <returns></returns>
        static int GetUnitsPerColumn(float viewportWorldHeight, int worldSizeTolerance = 5)
        {
            int digit = (int)(Mathf.Log(viewportWorldHeight / worldSizeTolerance, 2) / 3);
            digit = Mathf.Max(digit, 0);
            int step = (int)(Mathf.Log(viewportWorldHeight / worldSizeTolerance, 2) % 3);
            int baseUnit = 1;
            switch (step)
            {
                case 0:
                    baseUnit = 1;
                    break;
                case 1:
                    baseUnit = 2;
                    break;
                case 2:
                    baseUnit = 5;
                    break;
            }
            return baseUnit * (int)Mathf.Pow(10, digit);
        }

        static int GetFirstDigit(int i)
        {
            if (i >= 100000000) i /= 100000000;
            if (i >= 10000) i /= 10000;
            if (i >= 100) i /= 100;
            if (i >= 10) i /= 10;
            return i;
        }

        static int GetFirstNDigits(int number, int n)
        {
            // this is for handling negative numbers, we are only insterested in postitve number
            number = Mathf.Abs(number);
            // special case for 0 as Log of 0 would be infinity
            if (number == 0)
                return number;
            // getting number of digits on this input number
            int numberOfDigits = (int)Mathf.Floor(Mathf.Log10(number) + 1);
            // check if input number has more digits than the required get first N digits
            if (numberOfDigits >= n)
                return (int)(number / Mathf.Pow(10, numberOfDigits - n));
            else
                return number;
        }
    }

}