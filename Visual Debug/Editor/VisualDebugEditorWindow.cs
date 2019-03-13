using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Maumer;
using Maumer.Internal;

namespace TP.Experimental
{
    public class VisualDebugEditorWindow : EditorWindow
    {
        private static bool displayAll;
        private static Dictionary<Object, DebugDataEditorProperties> dataWatchDict = new Dictionary<Object, DebugDataEditorProperties>();

        [MenuItem("Tools/TP/Visual Debugger")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(VisualDebugEditorWindow), false, "Visual Debugger");
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
           SceneView.onSceneGUIDelegate -= OnSceneGUI;
           SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        private void Update()
        {
            OnUpdate();
        }

        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show All"))
            {
                ShowAll();
            }
            if (GUILayout.Button("Hide All"))
            {
                HideAll();
            }
            EditorGUILayout.EndHorizontal();
            foreach (var entry in dataWatchDict)
            {
                // EditorGUILayout.LabelField(entry.Value.data.loggerName, EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                entry.Value.unfold = EditorGUILayout.Foldout(entry.Value.unfold, entry.Value.data.loggerName);
                entry.Value.show = EditorGUILayout.Toggle("Show", entry.Value.show);
                EditorGUILayout.EndHorizontal();
                if (entry.Value.unfold)
                {
                    EditorGUI.BeginDisabledGroup(!entry.Value.show || entry.Value.data.frames.Count != 0);

                    GUILayout.Label(string.Format("Frame {0} of {1}", entry.Value.currentFrameIndex + 1, entry.Value.data.frames.Count));

                    EditorGUI.BeginChangeCheck();
                    entry.Value.currentFrameIndex = EditorGUILayout.IntSlider("Frame index",
                    entry.Value.currentFrameIndex,
                    0, entry.Value.data.frames.Count - 1
                    );
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("< Prev"))
                    {
                        entry.Value.currentFrameIndex = Mathf.Clamp(--entry.Value.currentFrameIndex, 0, entry.Value.data.frames.Count - 1);
                    }
                    if (GUILayout.Button("Next >"))
                    {
                        entry.Value.currentFrameIndex = Mathf.Clamp(++entry.Value.currentFrameIndex, 0, entry.Value.data.frames.Count - 1);
                    }
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        SceneView.RepaintAll();
                    }

                    if (entry.Value.currentFrame != null)
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUILayout.LabelField("Description: ");
                        EditorGUILayout.HelpBox(entry.Value.data.frames[entry.Value.currentFrameIndex].description, MessageType.None);
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUI.EndDisabledGroup();
                }
            }

        }

        private void OnUpdate()
        {
            UpdateDebugData();

        }

        private void UpdateDebugData()
        {
            if (!Application.isPlaying && VisualDebug.debugDatas == null)
            {
                return;
            }

            for (int i = 0; i < VisualDebug.debugDatas.Count; i++)
            {
                // add unique debug data to watch list
                if (!dataWatchDict.ContainsKey(VisualDebug.debugDatas[i].logger))
                {
                    dataWatchDict.Add(VisualDebug.debugDatas[i].logger,
                    new DebugDataEditorProperties(VisualDebug.debugDatas[i]));
                }
            }
            // Repaint the editor window to display new information
            // TODO: Is there a way to do this only when necessary??
            this.Repaint();

        }

        protected void OnSceneGUI(UnityEditor.SceneView sceneView)
        {
            if (!Application.isPlaying || VisualDebug.debugDatas == null)
            {
                return;
            }
            foreach (var entry in dataWatchDict)
            {
                if (entry.Value.show)
                {
                    for (int j = 0; j < entry.Value.data.frames.Count; j++)
                    {
                        entry.Value.data.frames[j].Draw(
                            entry.Value.currentFrameIndex
                        );
                    }
                }

            }
        }

        private void ShowAll()
        {
            foreach (var entry in dataWatchDict)
            {
                entry.Value.show = true;
            }
        }

        private void HideAll()
        {
            foreach (var entry in dataWatchDict)
            {
                entry.Value.show = false;
            }
        }

        private class DebugDataEditorProperties
        {
            public DebugData data;
            public Frame currentFrame
            {
                get
                {
                    if (data.frames == null || data.frames.Count == 0)
                        return null;
                    return data.frames[currentFrameIndex];
                }
            }
            public int currentFrameIndex;
            public bool show;
            public bool unfold;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="data">the data to bind with</param>
            public DebugDataEditorProperties(DebugData data)
            {
                this.data = data;
            }
        }
    }

}
