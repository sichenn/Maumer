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
                RulerSceneGUI.drawMode =
                    (RulerSceneGUI.DrawMode)EditorPrefs.GetInt("RulerSceneGUI_drawMode", 0);
                RulerSceneGUI.drawSpace =
                    (RulerSceneGUI.DrawSpace)EditorPrefs.GetInt("RulerSceneGUI_drawSpace", 0);
                RulerSceneGUI.drawDirection =
                    (RulerSceneGUI.DrawDirection)EditorPrefs.GetInt("RulerSceneGUI_drawDirection", 0);
                RulerSceneGUI.startAngle =
                    EditorPrefs.GetFloat("RulerSceneGUI_startAngle", 0);
                RulerSceneGUI.rulerDivisions =
                    EditorPrefs.GetInt("RulerSceneGUI_rulerDivisions", 0);
                RulerSceneGUI.showCursor =
                    EditorPrefs.GetBool("RulerSceneGUI_showCursor", true);
                RulerSceneGUI.showRuler =
                    EditorPrefs.GetBool("RulerSceneGUI_showRuler", true);

            }
            else
            {
                EditorPrefs.SetInt("RulerSceneGUI_drawMode", (int)RulerSceneGUI.drawMode);
                EditorPrefs.SetInt("RulerSceneGUI_drawSpace", (int)RulerSceneGUI.drawSpace);
                EditorPrefs.SetInt("RulerSceneGUI_drawDirection", (int)RulerSceneGUI.drawDirection);
                EditorPrefs.SetFloat("RulerSceneGUI_startAngle", RulerSceneGUI.startAngle);
                EditorPrefs.SetInt("RulerSceneGUI_rulerDivisions", RulerSceneGUI.rulerDivisions);
                EditorPrefs.SetBool("RulerSceneGUI_showCursor", RulerSceneGUI.showCursor);
                EditorPrefs.SetBool("RulerSceneGUI_showRuler", RulerSceneGUI.showRuler);
            }
            Menu.SetChecked(windowPath, enabled);
            EditorPrefs.SetBool(windowPath, enabled);
            RulerSceneGUI.show = enabled;
        }
    }

}