using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO 12/23 이거 툴한번 만들어보기

public class SnapShotTool : EditorWindow
{

    public GameObject objectToSnapshot;
    [HideInInspector]
    public Color backgroundColor = Color.clear;
    [HideInInspector]
    public Vector3 pos = new Vector3(0, 0, 1);
    [HideInInspector]
    public Vector3 rot = new Vector3(345.8529f, 313.8297f, 14.28433f);
    [HideInInspector]
    public Vector3 scale = new Vector3(1, 1, 1);

    private const string prefabsPath = "Prefabs/BuildObject/";

    private const string savePath = "Assets/SnapShots";

    Object[] snapShotObjects;

    //private SnapshotCamera snapshotCamera;
    //public Texture2D texture;

    [MenuItem("Tools/SnapShotTool")]
    static void Init()
    {
        SnapShotTool snapShot = (SnapShotTool)EditorWindow.GetWindow(typeof(SnapShotTool));

        //snapshotCamera = SnapshotCamera.MakeSnapshotCamera();

        snapShot.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("PreView ScreenShot", EditorStyles.boldLabel);

        GUILayoutOption[] options = new[]
        {
        GUILayout.Width (100),
        GUILayout.Height (30)
        };
        GUILayout.Space(10);

        //if (!Directory.Exists(path))
        //   Directory.CreateDirectory(path);

        if (GUILayout.Button("SnapShot", options))
        {
            LoadAllPrefabs();

            // for(int i= 0; i < snapShotObjects.Length; i++)
            // {
            //     Texture2D icon = UnityEditor.AssetPreview.GetAssetPreview(snapShotObjects[i]);
            //     File.WriteAllBytes(Application.dataPath + "/Snapshots/" + snapShotObjects[i].name + ".png", icon.EncodeToPNG());
            // }

            for (int i = 0; i < snapShotObjects.Length; i++)
            {
                SavePNG(snapShotObjects[i]);
            }

            AssetDatabase.Refresh();
        }
    }

    private string SavePath(string objName)
    {
        return Application.dataPath + "/Snapshots/" + objName + ".png";
    }

    private void SavePNG(Object obj)
    {
        string path = SavePath(obj.name);
        Texture2D icon = UnityEditor.AssetPreview.GetAssetPreview(obj);
        File.WriteAllBytes(path, icon.EncodeToPNG());
        Debug.Log($"SavePNGFile : " + path);
    }

    void LoadAllPrefabs()
    {
        snapShotObjects = Resources.LoadAll<Object>(prefabsPath);
        Debug.Log($"스냅샷 오브젝트 배열 길이 : " + snapShotObjects.Length);
    }

}

