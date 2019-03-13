using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Maumer
{
    [CustomEditor(typeof(ScreenRecorder))]
    public class ScreenRecorderEditor : Editor
    {

        private ScreenRecorder recorder
        {
            get
            {
                return target as ScreenRecorder;
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
