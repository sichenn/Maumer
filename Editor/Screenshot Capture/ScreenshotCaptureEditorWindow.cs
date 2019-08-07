using System;
using UnityEngine;
using UnityEditor;

namespace Maumer
{
    public class ScreenshotCaptureEditorWindow : EditorWindow
    {

        private static int s_SuperSize;

        [MenuItem("Window/Maumer/Screenshot Capture")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ScreenshotCaptureEditorWindow), false, "Screenshot Capture");
        }

        void OnGUI()
        {
            s_SuperSize = EditorGUILayout.IntField(new GUIContent("Super Size", "	Factor by which to increase resolution."), s_SuperSize);
            s_SuperSize = Mathf.Max(1, s_SuperSize);
            if (GUILayout.Button("Capture"))
            {
                var path = EditorUtility.SaveFilePanel(
                    "Save screenshot",
                    "",
                    DateTime.Now.ToString("dd.MM.yyyy hh-mm-ss") + ".png",
                    "png");
                if (path.Length != 0)
                {
                    ScreenCapture.CaptureScreenshot(path, s_SuperSize);
                }
            }
        }
    }
}
