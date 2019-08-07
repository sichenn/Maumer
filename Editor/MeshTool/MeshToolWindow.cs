using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Maumer
{
    /// <summary>
    /// Usage:
    /// Goto Window->MeshTool. You can use this tool to show some information about a mesh
    /// (such as its vert count, normals, etc..)
    /// </summary>
    public class MeshToolWindow : EditorWindow
    {
        private MeshFilter meshFilter;
        private Mesh mesh;
        private bool showVertices = false;
        private bool showNormals = false;
        private static Vector3[] verticePos;
        private static Mesh currentMesh;

        [MenuItem("Window/Maumer/Mesh Tool")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MeshToolWindow), false, "Mesh Tool");
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        }

        private void OnGUI()
        {
            var newMeshFilter = (MeshFilter)EditorGUILayout.ObjectField("Mesh Filter/ Mesh", meshFilter, typeof(MeshFilter), true);
            if (newMeshFilter != null && meshFilter != newMeshFilter)
            {
                meshFilter = newMeshFilter;
            }
            if (meshFilter)
            {
                if (mesh != meshFilter.sharedMesh)
                    mesh = meshFilter.sharedMesh;
                if (mesh)
                {
                    showMeshInfo();
                    showMeshVisualizationOptions();
                    showOptions();
                }

            }
        }

        /// <summary>
        /// shows the number of vertices, triangles, normals and uvs on the mesh
        /// </summary>
        private void showMeshInfo()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Verts: " + mesh.vertexCount);
            var normals = mesh.normals;
            var normalInfo = normals == null ? "normals: null" : "normals: " + normals.Length;
            EditorGUILayout.LabelField(normalInfo);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tris: " + mesh.triangles.Length);
            EditorGUILayout.LabelField("UVs: " + mesh.uv.Length);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// shows vertices and normals of this object in the scene view
        /// </summary>
        private void showMeshVisualizationOptions()
        {
            var VerticeButtonMessage = showVertices ? "Hide vertices" : "Show vertices";
            var NormalButtonMessage = showNormals ? "Hide normals" : "Show normals";

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent(VerticeButtonMessage, "Works best when vertexCount is less than 500")))
                showVertices = !showVertices;
            if (GUILayout.Button(new GUIContent(NormalButtonMessage, "Works best when vertexCount is less than 500")))
                showNormals = !showNormals;
            EditorGUILayout.EndHorizontal();
        }

        private void showOptions()
        {
            if (GUILayout.Button("Optimize"))
            {
                MeshUtility.Optimize(mesh);
            }
            if (GUILayout.Button("Save Mesh"))
            {
                saveMesh(mesh);
            }
        }

        /// <summary>
        /// Save the chosen mesh as an asset for reuse
        /// </summary>
        /// <param name="mesh"></param>
        private void saveMesh(Mesh mesh)
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Mesh Data", mesh.name, "asset", "WAT IS MESSAGE");
            if (string.IsNullOrEmpty(path)) return;
            Mesh saveMeshInstance = Object.Instantiate(mesh) as Mesh;
            AssetDatabase.CreateAsset(saveMeshInstance, path);
            AssetDatabase.SaveAssets();
        }


        public static void DrawMeshVertices(Transform transform, Mesh mesh)
        {
            if (currentMesh != mesh)
            {
                {
                    currentMesh = mesh;
                    verticePos = new Vector3[mesh.vertexCount];

                    for (int i = 0; i < currentMesh.vertexCount; i++)
                    {
                        var verticeOriginalPos = transform.TransformPoint(mesh.vertices[i]);
                        verticePos[i] = verticeOriginalPos + Random.insideUnitSphere * 0.1f;
                    }
                }

                for (int i = 0; i < verticePos.Length; i++)
                {
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.Lerp(Color.white, Color.black, i / verticePos.Length - 1);
                    Handles.Label(verticePos[i], i.ToString(), style);
                }
                HandleUtility.Repaint();
            }
        }

        public static void DrawMeshNormals(Transform transform, Mesh mesh)
        {
            for (int i = 0; i < mesh.normals.Length && i < mesh.vertices.Length; i++)
            {
                Vector3 normalColorXYZ = mesh.normals[i];
                normalColorXYZ.x = Mathf.Abs(normalColorXYZ.x);
                normalColorXYZ.x = Mathf.Abs(normalColorXYZ.y);
                normalColorXYZ.x = Mathf.Abs(normalColorXYZ.z);

                Handles.color = new Color(normalColorXYZ.x, normalColorXYZ.y, normalColorXYZ.z);
                Handles.DrawLine(transform.TransformPoint(mesh.vertices[i]),
                transform.TransformPoint(mesh.vertices[i]) + mesh.normals[i] * 0.5f * HandleUtility.GetHandleSize(mesh.vertices[i]));
            }
            HandleUtility.Repaint();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (meshFilter && mesh)
            {
                if (showVertices)
                {
                    DrawMeshVertices(meshFilter.transform, mesh);
                }
                if (showNormals)
                {
                    DrawMeshNormals(meshFilter.transform, mesh);
                }
            }
        }
    }
}

