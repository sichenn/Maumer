using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

namespace Maumer
{
    
public class CubemapGeneratorEditorWindow : EditorWindow
{
    public RenderTexture renderTexture;
    public Camera cubemapSourceCamera;
    // this camera strips all the excessive scripts from the source camera (such as post processing effects)
    private Camera cameraDouble;
    private GameObject cameraDoubleGO;

    [MenuItem("Tools/Cubemap Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CubemapGeneratorEditorWindow), false, "Cubemap Generator");
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (!cameraDoubleGO)
        {
            cameraDoubleGO = new GameObject();
            cameraDoubleGO.name = "camera double (tmp)";
            cameraDoubleGO.SetActive(false);
            // cameraDoubleGO.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    private void OnGUI()
    {
        showCameraField();

        if (cubemapSourceCamera)
        {
            showSettings();
        }
    }

    private void showCameraField()
    {
        cubemapSourceCamera = (Camera)EditorGUILayout.ObjectField(new GUIContent("Source Camera"), cubemapSourceCamera, typeof(Camera), true);
    }

    private void updateCameraSettings()
    {

        if (cameraDoubleGO.GetComponent<Camera>() == null)
            cameraDouble = cameraDoubleGO.AddComponent<Camera>();
        cameraDouble.CopyFrom(cubemapSourceCamera);
        cameraDouble.transform.position = cubemapSourceCamera.transform.position;
        cameraDouble.transform.rotation = cubemapSourceCamera.transform.rotation;
    }

    private void showSettings()
    {
        renderTexture = (RenderTexture)EditorGUILayout.ObjectField(new GUIContent("Output Render Texture"), renderTexture, typeof(RenderTexture), true);
        if (GUILayout.Button("Save"))
        {
            saveRenderTexture(renderTexture);
        }
    }

    private void saveRenderTexture(RenderTexture renderTexture)
    {
        cameraDoubleGO.SetActive(true);
        // TODO: Support depth settings

        renderTexture.Create();
        updateCameraSettings();

        if (!cameraDouble.RenderToCubemap(renderTexture, 63, Camera.MonoOrStereoscopicEye.Mono))
            Debug.LogError("Camera failed to render cubemap");
        cameraDoubleGO.SetActive(false);
        // ----------------------------------------------------Legacy----------------------------------------------------
        // ----One face would be missing if you use the code below. Reason unknown-----------------------------
        // renderTexture = new RenderTexture(cubemapResolution, cubemapResolution, 16);
        // renderTexture.dimension = TextureDimension.Cube;

        // if (generateMipMaps)
        //     renderTexture.GenerateMips();

        // var path = EditorUtility.SaveFilePanelInProject("Save RenderTexture", renderTexture.name, "renderTexture", "WAT IS MESSAGE");
        // if (string.IsNullOrEmpty(path)) return;
        // cubemapSourceCamera.RenderToCubemap(testRenderTexture, 63, Camera.MonoOrStereoscopicEye.Mono);
        // if (!cubemapSourceCamera.RenderToCubemap(renderTexture, 63, Camera.MonoOrStereoscopicEye.Mono))
        //     Debug.LogError("Camera failed to render cubemap");
        // AssetDatabase.CreateAsset(renderTexture, path);
        // AssetDatabase.SaveAssets();
    }

    [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
    static void drawCameraFrustrum(Camera scr, GizmoType gizmoType)
    {
        Transform scrTransform = scr.transform;
        Gizmos.matrix = Matrix4x4.TRS(scrTransform.position, scrTransform.rotation, scrTransform.lossyScale);
        Gizmos.DrawFrustum(new Vector3(0, 0, scr.nearClipPlane), scr.fieldOfView, scr.farClipPlane - scr.nearClipPlane, scr.nearClipPlane, scr.aspect);
    }
}

}