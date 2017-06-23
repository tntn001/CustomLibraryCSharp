#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(LoadingTexture))]
[DisallowMultipleComponent]
public class LoadingTextureEditor : Editor
{
    #region Texture

    SerializedProperty ListTextAssetTexture;
    SerializedProperty IsCreateNewTexture;
    SerializedProperty IsDeleteTex;
    SerializedProperty IsGetFileTex;
    SerializedProperty CurrentIndexTexProcess;
    SerializedProperty PathTex;
    SerializedProperty IsSprite;
    SerializedProperty IsCustomFolder;
    SerializedProperty PathCustom;
    SerializedProperty ListTextAsset;

    #endregion

    [SerializeField]
    private List<string> m_listTextAssetTexture;
    [SerializeField]
    private List<string> m_listTextAssetSprite;

    SerializedProperty ScrollView;
    GUIStyle GetFileStyle;

    void OnEnable()
    {
        #region Texture
        ListTextAssetTexture = serializedObject.FindProperty("m_listNameTextAssetTexture");
        IsCreateNewTexture = serializedObject.FindProperty("m_isCreateNewTex");
        IsDeleteTex = serializedObject.FindProperty("m_isDeleteTex");
        CurrentIndexTexProcess = serializedObject.FindProperty("m_nCurrIndexTexProcess");
        IsGetFileTex = serializedObject.FindProperty("m_isGetFileTex");
        PathTex = serializedObject.FindProperty("m_sPathTex");     
        IsCustomFolder = serializedObject.FindProperty("m_isCustomFolder");
        PathCustom = serializedObject.FindProperty("m_sPathCustom");
        ListTextAsset = serializedObject.FindProperty("m_ListTextAssetTexture");
        #endregion
     
        IsSprite = serializedObject.FindProperty("m_listIsSprite");
        ScrollView = serializedObject.FindProperty("m_v2ScrollView");
        ScrollView.vector2Value = Vector2.zero;
    }

    public override void OnInspectorGUI()
    {        
        if (ListTextAssetTexture != null)
        {
            GUILayout.Label("Texture: ");
            ScrollView.vector2Value = GUILayout.BeginScrollView(ScrollView.vector2Value, true, false, GUILayout.Height(600));
            for (int i = 0; i < ListTextAssetTexture.arraySize; i++)
            {
                GUILayout.BeginHorizontal();
                if (CurrentIndexTexProcess.intValue == i)
                    GUI.color = Color.red;
                if (ListTextAsset.GetArrayElementAtIndex(i) == null)
                {
                    GUILayout.Label("ID " + (i + 1) + ": Missing Object");
                }
                else
                {
                    GUILayout.Label("ID " + (i + 1) + ": " + ListTextAssetTexture.GetArrayElementAtIndex(i).stringValue);
                }
                GUILayout.FlexibleSpace();               
                if (GUILayout.Button("Get", GUILayout.Width(40)))
                {                    
                    PathTex.stringValue = EditorUtility.OpenFilePanel("Load texture", PathTex.stringValue.Replace(Application.dataPath, "Assets/"), "");
                    CurrentIndexTexProcess.intValue = i;
                    if (PathTex.stringValue != null)
                        IsGetFileTex.boolValue = true;
                }
                if (GUILayout.Button("Del"))
                {
                    IsDeleteTex.boolValue = true;
                    CurrentIndexTexProcess.intValue = i;
                }    
                if (IsSprite.arraySize > i && IsSprite.GetArrayElementAtIndex(i) != null)
                    IsSprite.GetArrayElementAtIndex(i).boolValue = GUILayout.Toggle(IsSprite.GetArrayElementAtIndex(i).boolValue, "Sprite");
                if (GUILayout.Button("..."))
                {
                    EditorGUIUtility.PingObject(ListTextAsset.GetArrayElementAtIndex(i).objectReferenceValue);
                }
                GUI.color = Color.white;
                GUILayout.Space(20);
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("New", GUILayout.Width(150)))
            {
                ScrollView.vector2Value += new Vector2(0, 50);
                IsCreateNewTexture.boolValue = true;                
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.Label("***********************************************************************************************************************", GUILayout.MinWidth(0));
        }
        bool oldIsCustomFolder = IsCustomFolder.boolValue;
        IsCustomFolder.boolValue = GUILayout.Toggle(IsCustomFolder.boolValue, "Is Save Custom Folder");

        if (IsCustomFolder.boolValue)
        {
            if (!oldIsCustomFolder)
            {
                oldIsCustomFolder = true;
                PathCustom.stringValue = EditorUtility.OpenFolderPanel("Save file at", PathCustom.stringValue.Replace(Application.dataPath, "Assets/"), "");
            }
            GUILayout.Label("Current save folder: " + PathCustom.stringValue.Replace(Application.dataPath, "Assets/"));
        }
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}
#endif
