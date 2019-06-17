using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Maumer
{
    public class RaycastDebuggerWindow : EditorWindow
    {
        public LayerMask layerMask;
        public CastType castType;
        public BoxcastParameters boxcastParameters;
        private Vector3 m_mouseDownPos;
        private Vector3 m_worldMousePos;
        private bool casting;
        private RaycastHit2D m_hit;

        [MenuItem("Window/Maumer/Raycast Debugger")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(RaycastDebuggerWindow), false, "Raycast Debugger");
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (!Application.isPlaying)
            {
                // force repaint scene view in editor mode to avoid lagging
                SceneView.RepaintAll();
            }
        }

        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        void OnGUI()
        {
            EditorGUILayout.HelpBox("Press \"R\" to turn on/of a ray", UnityEditor.MessageType.Info);
            // layerMask = EditorGUILayout.LayerField("Layer Mask", layerMask);
            EditorGUI.BeginChangeCheck();
            var inputLayerMask = EditorGUILayout.MaskField("Layer Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "changed RaycastDebugger layermask");
                layerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(inputLayerMask);
            }
            castType = (CastType)EditorGUILayout.EnumPopup("Cast Type", castType);
            switch (castType)
            {
                case CastType.Boxcast:

                    boxcastParameters.OnGUI();
                    break;
            }
        }

        protected void OnSceneGUI(SceneView sceneView)
        {
            CastRay();
            DisplayRayhitData();
        }

        private void CastRay()
        {
            // start dragging a ray when R is pressed
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R)
            {
                m_mouseDownPos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin; ;
                m_mouseDownPos.z = 0;
                casting = !casting;
            }

            // cast ray towards where the mouse currently is
            if (casting)
            {
                // cast a ray from origin to current mouse position
                m_worldMousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin; ;
                m_worldMousePos.z = 0;
                switch (castType)
                {
                    case CastType.Raycast:
                        m_hit = Physics2D.Raycast((Vector2)m_mouseDownPos,
                        (m_worldMousePos - m_mouseDownPos).normalized,
                         (m_worldMousePos - m_mouseDownPos).magnitude, layerMask.value);
                        // display ray in red
                        Handles.color = Color.red;
                        Handles.DrawLine(m_mouseDownPos,
                        m_worldMousePos);

                        // if hit exists, draw a green indicator
                        if (m_hit.collider != null)
                        {
                            Handles.color = Color.green;
                            Handles.DrawLine(m_mouseDownPos, m_hit.point);
                            Handles.DrawWireDisc(m_hit.point, Vector3.forward,
                            0.1f * HandleUtility.GetHandleSize(m_hit.point));
                        }
                        break;
                    case CastType.Boxcast:
                        m_hit = Physics2D.BoxCast((Vector2)m_mouseDownPos,
                        boxcastParameters.size, boxcastParameters.angle,
                        (m_worldMousePos - m_mouseDownPos).normalized,
                        (m_worldMousePos - m_mouseDownPos).magnitude, layerMask.value);
                        // display ray in red
                        Handles.color = Color.red;
                        Handles.DrawLine(m_mouseDownPos,
                        m_worldMousePos);
                        var defaultMatrix = Handles.matrix;
                        Handles.matrix = Matrix4x4.TRS(
                            m_worldMousePos,
                            Quaternion.AngleAxis(boxcastParameters.angle, Vector3.forward),
                            Vector3.one);
                        Handles.DrawWireCube(Vector2.zero, boxcastParameters.size);
                        Handles.matrix = defaultMatrix;

                        // if hit exists, draw a green indicator
                        if (m_hit.collider != null)
                        {
                            Handles.color = Color.green;
                            Handles.DrawLine(m_mouseDownPos, m_hit.point);
                            Handles.DrawWireDisc(m_hit.point, Vector3.forward,
                            0.1f * HandleUtility.GetHandleSize(m_hit.point));
                        }
                        break;
                }

            }
        }

        private void DisplayRayhitData()
        {
            if (m_hit.collider != null)
            {
                Handles.BeginGUI();

                var defaultGUIBGColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.Lerp(defaultGUIBGColor, Color.clear, 0.25f);

                m_worldMousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                GUILayout.Window(2,
                new Rect(
                m_worldMousePos.x,
                m_worldMousePos.y,
                100, 100),
                (id) =>
                {
                    GUILayout.Label("Point: " + m_hit.point + "\n" +
                    "Normal: " + m_hit.normal);
                }, "Raycast Data");
                GUI.backgroundColor = defaultGUIBGColor;
                Handles.EndGUI();
                // Handles.
                // Handles.Label();
            }
        }

        public enum CastType
        {
            Raycast,
            Boxcast
        }

        [System.Serializable]
        public class BoxcastParameters
        {
            public Vector2 size = Vector2.one;
            public float angle;

            internal void OnGUI()
            {
                size = EditorGUILayout.Vector2Field("size", size);
                angle = EditorGUILayout.FloatField("angle", angle);
            }
        }
    }
}
