using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Maumer
{
    public class MotionTrackerEditorWindow : EditorWindow
    {
        private static bool showPath;
        private static bool showDeltaPosition;
        // TODO: You probably need a better name for this
        private static int showInterval;
        private readonly Color k_pressedColor = new Color(0.4f, 0.4f, 0.4f);

        [MenuItem("Window/Maumer/MotionTracker")]
        static void Init()
        {
            var window = (MotionTrackerEditorWindow)EditorWindow.GetWindow(typeof(MotionTrackerEditorWindow));
            window.Show();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            // Remove delegate listener if it has previously been assigned.
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
            // Add (or re-add) the delegate.
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        }

        private void Update()
        {
            MotionTracker.Update();
        }

        private void OnGUI()
        {
            // Data Panel
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
            MotionTracker.target = (Transform)EditorGUILayout.ObjectField("Target", MotionTracker.target, typeof(Transform), true);
            EditorGUILayout.LabelField("Time:", MotionTracker.totalRecordTime.ToString());
            EditorGUILayout.LabelField("Speed:", MotionTracker.targetSpeed.ToString());
            EditorGUILayout.LabelField("Delta Position:", MotionTracker.targetDeltaPosition.ToString());

            EditorGUILayout.BeginHorizontal();

            GUI.color = MotionTracker.IsRecording ? k_pressedColor : GetDefaultSkinColor();
            if (GUILayout.Button("Start"))
            {
                MotionTracker.Start();
            }
            GUI.color = !MotionTracker.IsRecording ? k_pressedColor : GetDefaultSkinColor();
            if (GUILayout.Button("Pause"))
            {
                MotionTracker.Stop();
            }
            GUI.color = GetDefaultSkinColor();
            if (GUILayout.Button("Clear"))
            {
                MotionTracker.Clear();
            }
            EditorGUILayout.EndHorizontal();

            // Inspection Panel
            EditorGUILayout.LabelField("Inspection", EditorStyles.boldLabel);
            showPath = EditorGUILayout.Toggle("Show Path", showPath);
            showDeltaPosition = EditorGUILayout.Toggle("Show Delta Position", showDeltaPosition);
        }

        private static Color GetDefaultSkinColor()
        {
            return EditorGUIUtility.isProSkin
                ? new Color32(56, 56, 56, 255)
                : new Color32(194, 194, 194, 255);
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (showPath)
            {
                if (MotionTracker.positions != null)
                {
                    for (int i = 0; i < MotionTracker.positions.Count; i++)
                    {
                        Handles.DrawWireDisc(MotionTracker.positions[i], Vector3.back, 0.1f);
                    }
                }
            }
            if (showDeltaPosition)
            {
                // Not implemented
            }
        }
    }

}