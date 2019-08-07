using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Maumer
{
    [CustomEditor(typeof(ScreenshotCapture))]
    public class ScreenRecorderEditor : Editor
    {

        private ScreenshotCapture recorder
        {
            get
            {
                return target as ScreenshotCapture;
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Capture"))
            {
                recorder.CaptureScreenshot();
            }
        }
    }

}
