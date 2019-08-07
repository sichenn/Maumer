using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Maumer
{

    [InitializeOnLoad]
    public static class RulerSceneGUIEditor
    {
        private const string windowPath = "Window/Maumer/Ruler";

        static RulerSceneGUIEditor()
        {
            RulerSceneGUI.show = EditorPrefs.GetBool(windowPath, false);
            EditorApplication.delayCall += () =>
            {
                ConfigurePreferences(RulerSceneGUI.show);
            };
        }

        [MenuItem(windowPath)]
        private static void ToggleAction()
        {
            ConfigurePreferences(!RulerSceneGUI.show);
            SceneView.RepaintAll();
        }

        private static void ConfigurePreferences(bool enabled)
        {
            if (enabled)
            {
                RulerSceneGUI.startAngle =
                    EditorPrefs.GetFloat("RulerSceneGUI_startAngle", 0);
                RulerSceneGUI.rulerDivisions =
                    EditorPrefs.GetInt("RulerSceneGUI_rulerDivisions", 0);
            }
            else
            {
                EditorPrefs.SetFloat("RulerSceneGUI_startAngle", RulerSceneGUI.startAngle);
                EditorPrefs.SetInt("RulerSceneGUI_rulerDivisions", RulerSceneGUI.rulerDivisions);
            }
            Menu.SetChecked(windowPath, enabled);
            EditorPrefs.SetBool(windowPath, enabled);
            RulerSceneGUI.show = enabled;
        }
    }

}