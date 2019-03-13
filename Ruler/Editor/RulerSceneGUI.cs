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
        public static bool showCursor = true;
        public static bool showRuler = true;
        public static bool smartSnap = false;
        public static bool settingRulersHorizontal = false;
        public static bool settingRulersVertical = false;
        public static DrawMode drawMode;
        public static DrawSpace drawSpace;
        public static DrawDirection drawDirection;
        // TODO: Implement guides like in Photoshop
        private static List<float> verticalGuides;
        private static List<float> horizontalGuides;
        private static Texture2D m_RulerIconHorizontal = GetGizmo("../Gizmos/rulerSmall.png");
        private static Texture2D m_RulerIconVertical = GetGizmo("../Gizmos/rulerSmall_Vertical.png");
        private static int top = 0;
        private static Vector3 m_mousePosInWorld;
        private static Camera m_sceneViewCamera;
        private static Color k_lineColor = new Color(.22f, .22f, .22f);


        /// <summary>
        /// Called at startup
        /// </summary>
        static RulerSceneGUI()
        {
            verticalGuides = new List<float>();
            horizontalGuides = new List<float>();
            EditorApplication.update += Update;
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        static void Update()
        {
            //anything you want to do every frame..
            m_sceneViewCamera = SceneView.lastActiveSceneView.camera;
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("MOUSEUP UPDATE");
            }
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (show)
            {

                if (showCursor)
                {
                    DisplayCursor();
                }
                if (m_sceneViewCamera.orthographic)
                {
                    DisplayRulerBars();
                }
                if (drawMode == DrawMode.Straight)
                {
                    DisplayGuides();
                    DisplayToolbar();
                    SetGuides();
                }
                else if (drawMode == DrawMode.Circular)
                {
                    DisplayGuidesCircular();
                    DisplayToolbarCircular();
                    SetGuidesCircular();
                }

                DetectHotkeys();
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

        private static void DisplayToolbar()
        {
            Handles.BeginGUI();

            int size = 16;
            int currentX = 2;
            top = (int)Screen.height - size * 3 - 8;

            // Draw ruler toolbar
            GUI.Box(new Rect(0, top, Screen.width, size), "", EditorStyles.toolbar);

            // reset the color back to normal
            GUI.contentColor = Color.white;

            drawMode = (DrawMode)EditorGUI.EnumPopup(new Rect(currentX, top, size * 4, size), drawMode, EditorStyles.toolbarPopup);
            currentX += size * 4;

            drawSpace = (DrawSpace)EditorGUI.EnumPopup(new Rect(currentX, top, size * 6, size), drawSpace, EditorStyles.toolbarPopup);
            currentX += size * 6;

            // if it's not the pro skin, the icons are too bright, almost unseeable
            if (!EditorGUIUtility.isProSkin)
            {
                GUI.contentColor = Color.black;
            }
            settingRulersHorizontal = GUI.Toggle(new Rect(currentX, top, size * 2, size), settingRulersHorizontal,
            new GUIContent(m_RulerIconHorizontal, "Place Horizontal Guides"), EditorStyles.toolbarButton);
            currentX += size * 2;
            settingRulersVertical = GUI.Toggle(new Rect(currentX, top, size * 2, size), settingRulersVertical,
             new GUIContent(m_RulerIconVertical, "Place Vertical Guides"), EditorStyles.toolbarButton);
            //Draw an icon example
            //GUI.Label(new Rect(currentX, top+2, size, size), icon);

            Handles.EndGUI();
        }

        private static void DisplayToolbarCircular()
        {
            Handles.BeginGUI();

            int size = 16;
            int currentX = 2;
            top = (int)Screen.height - size * 3 - 8;

            // Draw ruler toolbar
            GUI.Box(new Rect(0, top, Screen.width, size), "", EditorStyles.toolbar);

            // reset the color back to normal
            GUI.contentColor = Color.white;

            drawMode = (DrawMode)EditorGUI.EnumPopup(new Rect(currentX, top, size * 4, size), drawMode, EditorStyles.toolbarPopup);
            currentX += size * 4;

            drawSpace = (DrawSpace)EditorGUI.EnumPopup(new Rect(currentX, top, size * 6, size), drawSpace, EditorStyles.toolbarPopup);
            currentX += size * 6;

            drawDirection = (DrawDirection)EditorGUI.EnumPopup(new Rect(currentX, top, size * 6, size), drawDirection, EditorStyles.toolbarPopup);
            currentX += size * 6;

            startAngle = EditorGUI.FloatField(new Rect(currentX, top, size * 12, size), "Start Angle", startAngle);
            startAngle = Mathf.Clamp(startAngle, 0, 360);
            currentX += size * 12;

            rulerDivisions = EditorGUI.IntField(new Rect(currentX, top, size * 12, size), "Divisions", rulerDivisions);
            rulerDivisions = Mathf.Clamp(rulerDivisions, 0, 360);
            currentX += size * 12 + size;

            showRuler = GUI.Toggle(new Rect(currentX, top, size * 5, size), showRuler, new GUIContent("Show Ruler", "Ctrl+R"), EditorStyles.toolbarButton);
            currentX += size * 5;

            showCursor = GUI.Toggle(new Rect(currentX, top, size * 5, size), showCursor, new GUIContent("Show Cursor", "Ctrl+Shift+R"), EditorStyles.toolbarButton);
            currentX += size * 5;

            // if it's not the pro skin, the icons are too bright, almost unseeable
            if (!EditorGUIUtility.isProSkin)
            {
                GUI.contentColor = Color.black;
            }
            settingRulersHorizontal = GUI.Toggle(new Rect(currentX, top, size * 5, size), settingRulersHorizontal, new GUIContent(m_RulerIconHorizontal, "Place Guides"), EditorStyles.toolbarButton);

            //Draw an icon example
            //GUI.Label(new Rect(currentX, top+2, size, size), icon);
            currentX += size + 6;

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

        private static void DetectHotkeys()
        {
            //hotkey
            if (Event.current.control && Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.R)
            {
                if (Event.current.shift)
                {
                    showCursor = !showCursor;
                }
                else
                {
                    showRuler = !showRuler;
                }
            }
        }

        private static void DisplayGuidesCircular()
        {
            Handles.DrawLine(Vector3.zero, 10000 * new Vector3(Mathf.Cos((startAngle) * Mathf.Deg2Rad), Mathf.Sin((startAngle) * Mathf.Deg2Rad), 0));
            if (rulerDivisions != 0)
            {
                var deltaAngle = drawDirection == DrawDirection.Clockwise ? -360f / rulerDivisions : 360f / rulerDivisions;
                switch (drawSpace)
                {
                    case DrawSpace.DrawGlobal:
                        Handles.color = Color.cyan;
                        for (int i = 1; i < rulerDivisions; i++)
                        {
                            Handles.DrawLine(Vector3.zero, 10000 * new Vector3(
                                Mathf.Cos((i * deltaAngle + startAngle) * Mathf.Deg2Rad),
                                Mathf.Sin((i * deltaAngle + startAngle) * Mathf.Deg2Rad),
                                0));
                        }
                        break;
                    case DrawSpace.DrawLocal:
                        Handles.color = Color.cyan;
                        var pos = SceneView.currentDrawingSceneView.camera.transform.position;
                        pos.z = 0;
                        for (int i = 1; i < rulerDivisions; i++)
                        {
                            Handles.DrawLine(pos, pos + 1000 * new Vector3(
                                Mathf.Cos((i * deltaAngle + startAngle) * Mathf.Deg2Rad),
                                Mathf.Sin((i * deltaAngle + startAngle) * Mathf.Deg2Rad),
                                0));
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void DisplayGuides()
        {
            Handles.color = Color.cyan;
            foreach (var horizontalGuide in horizontalGuides)
            {
                Handles.DrawLine(new Vector2(-m_sceneViewCamera.farClipPlane, horizontalGuide),
                new Vector2(m_sceneViewCamera.farClipPlane, horizontalGuide));
            }
            foreach (var verticalGuide in verticalGuides)
            {
                Handles.DrawLine(new Vector2(verticalGuide, -m_sceneViewCamera.farClipPlane),
                new Vector2(verticalGuide, m_sceneViewCamera.farClipPlane));
            }
        }

        /// <summary>
        /// Drawings a circular radius and displays coordinate info.
        /// local drawing has not been implemented yet
        /// </summary>
        private static void DisplayCursor(bool withGuidelines = true)
        {
            Handles.BeginGUI();

            var currentRectPos = Event.current.mousePosition + new Vector2(1, -1) * 8;
            var textLabelRect = new Rect(currentRectPos, Vector2.one * 100);
            var textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.white;
            m_mousePosInWorld = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

            //only the first two decimal digits are reserved
            var radius = ((Vector2)m_mousePosInWorld).magnitude;
            GUI.Label(textLabelRect, "radius: " + System.Math.Round(radius, 1), textStyle);
            textLabelRect = new Rect(currentRectPos += Vector2.down * 12, Vector2.one * 100);

            // the calculation is weird but it works. It could be better but I currently don't have the time to improve it
            var angle = drawDirection == DrawDirection.Clockwise ?
                (-Vector2.SignedAngle(Vector2.left, m_mousePosInWorld.normalized) + 180 + startAngle) % 360 :
                (Vector2.SignedAngle(Vector2.left, m_mousePosInWorld.normalized) + 180 - startAngle) % 360;
            GUI.Label(textLabelRect, "angle: " + System.Math.Round(angle, 1), textStyle);
            Handles.EndGUI();

            Handles.DrawWireCube(m_mousePosInWorld, Vector3.one * 0.1f * HandleUtility.GetHandleSize(m_mousePosInWorld));

            if (withGuidelines)
            {
                //draw Global mode
                Handles.DrawWireArc(Vector3.zero, Vector3.back, (Vector2)m_mousePosInWorld.normalized, drawDirection == DrawDirection.Clockwise ? -angle : angle, radius);
                Handles.DrawLine(Vector3.zero, m_mousePosInWorld);
                //todo: draw local mode
            }

            SceneView.RepaintAll();
        }

        private static void SetGuidesCircular()
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Handles.color = Color.cyan;
            var radius = ((Vector2)(m_mousePosInWorld)).magnitude;
            Handles.DrawWireArc(Vector3.zero, Vector3.back, (Vector2)m_mousePosInWorld.normalized, 360, radius);
            DisplayCursor(false);
            SceneView.RepaintAll();
        }

        private static void SetGuides()
        {
            Handles.BeginGUI();
            // Event.current.mousePosition;
            Handles.color = Color.white;
            var mousePos = Event.current.mousePosition;
            if (settingRulersHorizontal)
            {
                Handles.DrawLine(new Vector2(0, mousePos.y), new Vector2(Screen.width, mousePos.y));
                if (Event.current.type == EventType.MouseUp)
                {
                    horizontalGuides.Add(m_sceneViewCamera.ScreenToWorldPoint(mousePos).y);
                    settingRulersHorizontal = false;
                    Debug.Log("MOUSEUP");
                }
            }
            if (settingRulersVertical)
            {
                Handles.DrawLine(new Vector2(mousePos.x, 0), new Vector2(mousePos.x, Screen.height));
                if (Event.current.type == EventType.MouseUp)
                {
                    verticalGuides.Add(m_sceneViewCamera.ScreenToWorldPoint(mousePos).x);
                    settingRulersVertical = false;
                    Debug.Log("MOUSEUP");
                }
            }
            Handles.EndGUI();
            SceneView.RepaintAll();
        }

        private static Texture2D GetGizmo(string path)
        {
            string fullPath = RelativeToFullPath(path);
            Texture2D tex = AssetDatabase.LoadAssetAtPath(fullPath, typeof(Texture2D)) as Texture2D;
            if (tex == null)
            {
                tex = EditorGUIUtility.whiteTexture;
                Debug.Log("Couldn't load Gizmo tex " + fullPath);
            }
            return tex;
        }

        private static string RelativeToFullPath(string relativePath)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(0, true);
            System.Diagnostics.StackFrame frame = stackTrace.GetFrame(0);
            string fullPath = frame.GetFileName();

            // start from project root
            fullPath = fullPath.Substring(fullPath.IndexOf("Assets"));
            fullPath = fullPath.Replace('\\', '/');

            // remove script file from path
            fullPath = fullPath.Substring(0, fullPath.LastIndexOf('/'));

            //go up in file hierarchy
            while (relativePath.Length >= 3 && relativePath.Substring(0, 3) == "../")
            {
                fullPath = fullPath.Substring(0, fullPath.LastIndexOf('/'));
                relativePath = relativePath.Substring(3);
            }

            fullPath += '/' + relativePath;
            return fullPath;
        }

        public enum DrawSpace
        {
            DrawGlobal,
            DrawLocal
        }

        public enum DrawDirection
        {
            Clockwise,
            CounterClockwise
        }

        public enum DrawMode
        {
            Straight,
            Circular
        }
    }

}