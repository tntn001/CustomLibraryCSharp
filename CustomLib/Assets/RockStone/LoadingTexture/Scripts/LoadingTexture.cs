using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[ExecuteInEditMode]
public class LoadingTexture : MonoBehaviour
{
    #region Events

    public event Action OnLoadingComplete;
    public event Action<int> OnLoadStepbyStep;

    #endregion

    //******************************************************************************\\

    #region Runtime property

    [HideInInspector]
    [SerializeField]
    private Texture2D[] m_ListTexture;
    [HideInInspector]
    [SerializeField]
    private List<Sprite> m_listSprite;   

    private Vector3 m_v3OriginScaleMesh;
    private Vector3 m_v3OriginPositionMesh;
    private Vector3 m_v3OriginPositionLoadingBarSlider;

    #endregion

    //******************************************************************************\\

    #region Editor property

    #region Texture

    [HideInInspector]
    [SerializeField]
    private List<string> m_listNameTextAssetTexture;
    [HideInInspector]
    [SerializeField]
    private bool m_isCreateNewTex;
    [HideInInspector]
    [SerializeField]
    private bool m_isDeleteTex;
    [SerializeField]
    [HideInInspector]
    private bool m_isGetFileTex;
    [SerializeField]
    [HideInInspector]
    private string m_sPathTex;
    [HideInInspector]
    [SerializeField]
    private int m_nCurrIndexTexProcess;

    #endregion

    [SerializeField]
    [HideInInspector]
    private List<TextAsset> m_ListTextAssetTexture;
    [SerializeField]
    [HideInInspector]
    private List<Vector2> m_ListSizeTex;
    [SerializeField]
    [HideInInspector]
    private List<bool> m_listIsSprite;
    [SerializeField]
    [HideInInspector]
    private Vector2 m_v2ScrollView;
    readonly string _folder_save_binary_tex_file_ = "Assets/LoadingTexture";

    [HideInInspector]
    [SerializeField]
    private bool m_isCustomFolder;
    [HideInInspector]
    [SerializeField]
    private string m_sPathCustom;

    #endregion

    //******************************************************************************\\

    #region Runtime method

    public static LoadingTexture Instance
    {
        get{ return _instance; }
    }

    public bool IsLoadingComplete
    {
        get{ return m_isLoadingComplete; }
    }

    private bool m_isLoadingComplete;

    private static LoadingTexture _instance;
    private float m_nTotalFileNeededLoading;
    private float m_nCurrentFileWasLoaded;

    #region Public method

    public Texture2D GetTex(int id)
    {
        return m_ListTexture[id - 1];
    }

    public Texture2D GetTex(int id, out Vector2 size)
    {
        size = m_ListSizeTex[id - 1];
        return m_ListTexture[id - 1];
    }

    public Sprite GetSpr(int id)
    {
        return m_listSprite[id - 1];
    }

    public Vector2 GetSize(int id)
    {
        return m_ListSizeTex[id - 1];
    }

    #endregion

    #region Editor public method

    public Texture2D GetTexEditor(int id)
    {
        if (Application.isEditor && !Application.isPlaying)
        {            
            Texture2D tex = new Texture2D((int)m_ListSizeTex[id - 1].x, (int)m_ListSizeTex[id - 1].y, TextureFormat.RGBA32, false, false);
            tex.LoadImage(m_ListTextAssetTexture[id - 1].bytes);
            tex.filterMode = FilterMode.Point;
            return tex;
        }
        else
        {
            Debug.LogError("This script only use on editor");
            return null;
        }
    }

    public Texture2D GetTexEditor(int id, out Vector2 size)
    {
        size = new Vector2(0, 0);
        if (Application.isEditor && !Application.isPlaying)
        {            
            Texture2D tex = new Texture2D((int)m_ListSizeTex[id - 1].x, (int)m_ListSizeTex[id - 1].y, TextureFormat.RGBA32, false, false);
            tex.LoadImage(m_ListTextAssetTexture[id - 1].bytes);
            tex.filterMode = FilterMode.Point;
            size = m_ListSizeTex[id - 1];
            return tex;
        }
        else
        {
            Debug.LogError("This script only use on editor");
            return null;
        }
    }

    public Sprite GetSprEditor(int id)
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Texture2D tex = new Texture2D(4, 4, TextureFormat.RGBA32, false, false);
            tex.LoadImage(m_ListTextAssetTexture[id - 1].bytes);
            tex.filterMode = FilterMode.Point;
            return Sprite.Create(tex, new Rect(Vector2.zero, m_ListSizeTex[id - 1]), new Vector2(0.5f, 0.5f), 1, 0, SpriteMeshType.FullRect);
        }
        else
        {
            Debug.LogError("This script only use on editor");
            return null;
        }
    }

    #endregion

    void Awake()
    {
        if (Application.isPlaying)
        {
            _instance = this;           
            StartCoroutine(LoadingTex());
        }
        else
        {            
        }
    }

    IEnumerator LoadingTex()
    {
        m_isLoadingComplete = false;
        m_nCurrentFileWasLoaded = 0;            
        float percentLoad = 0;
        #if UNITY_EDITOR
        GUILoadingDone = false;
        #endif
        m_nTotalFileNeededLoading = m_ListTextAssetTexture != null ? m_ListTextAssetTexture.Count : 0;
        m_ListTexture = new Texture2D[m_ListTextAssetTexture.Count];
        m_listSprite = new List<Sprite>();
        float startLoadingTime = Time.time;
        for (int i = 0; i < m_ListTextAssetTexture.Count; i++)
        {  
            m_ListTexture[i] = new Texture2D((int)m_ListSizeTex[i].x, (int)m_ListSizeTex[i].y, TextureFormat.ARGB32, false, false);
            m_ListTexture[i].LoadImage(m_ListTextAssetTexture[i].bytes);
            m_ListTexture[i].name = m_listNameTextAssetTexture[i];   
            m_ListTexture[i].filterMode = FilterMode.Point;
            if (m_listIsSprite != null)
            {
                if (m_listIsSprite[i])
                {
                    Sprite spr = Sprite.Create(m_ListTexture[i], new Rect(Vector2.zero, m_ListSizeTex[i]), new Vector2(0.5f, 0.5f), 1, 0, SpriteMeshType.Tight);
                    m_listSprite.Add(spr);
                }
                else
                    m_listSprite.Add(null);
            }

            m_ListTexture[i].Apply(false, true);
            yield return new WaitForEndOfFrame();

            if (OnLoadStepbyStep != null)
            {
                OnLoadStepbyStep(i + 1);
            }          
            m_nCurrentFileWasLoaded++;
            percentLoad = m_nCurrentFileWasLoaded / m_nTotalFileNeededLoading;                  
            
            #if UNITY_EDITOR 
            GUILoadingString = "Loading .. " + (percentLoad * 100) + "% " + m_listNameTextAssetTexture[i];
            #endif
        }
        Debug.Log("Total time loading: " + (Time.time - startLoadingTime));
      
        m_isLoadingComplete = true;
        if (OnLoadingComplete != null)
        {
            OnLoadingComplete();
        }

        yield return new WaitForEndOfFrame();
    
        #if UNITY_EDITOR
        GUILoadingDone = true;
        #endif


        StopCoroutine(LoadingTex());
    }

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            FreeMemory();
        }
    }

    public void FreeMemory(Action callback = null)
    {
        if (m_ListTexture != null)
        {
            for (int i = 0; i < m_ListTexture.Length; i++)
            {
                m_ListTexture[i] = null;      
            }
            m_ListTexture = null;
          
            _instance = null;
            GameObject.Destroy(gameObject);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();

            if (callback != null)
                callback();
        } 
    }

    #if UNITY_EDITOR
    private string GUILoadingString;
    private bool GUILoadingDone;

    void OnGUI()
    {        
        if (!GUILoadingDone)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.magenta;
            style.fontSize = 30;
            GUI.Label(new Rect(0, 0, 200, 50), GUILoadingString, style);
        }       
    }
    #endif

    void Update()
    {
        #if UNITY_EDITOR
        if (Application.isEditor && !Application.isPlaying)
        {  
            // TEXTURE:
            if (m_isCreateNewTex)
            {               
                m_isCreateNewTex = false;
                m_listNameTextAssetTexture.Add("<----NONE---->");
                m_ListTextAssetTexture.Add(null);
                m_ListSizeTex.Add(Vector2.zero);   
                m_listIsSprite.Add(false);
            }
            if (m_isDeleteTex)
            {
                m_isDeleteTex = false;
                m_listNameTextAssetTexture.RemoveAt(m_nCurrIndexTexProcess);
                m_ListTextAssetTexture.RemoveAt(m_nCurrIndexTexProcess);
                m_ListSizeTex.RemoveAt(m_nCurrIndexTexProcess);
                m_listIsSprite.RemoveAt(m_nCurrIndexTexProcess);
            }
            if (m_isGetFileTex)
            {   
                m_isGetFileTex = false;
                m_ListTextAssetTexture[m_nCurrIndexTexProcess] = GetTexture(m_sPathTex);
            }        
        }
        #endif
    }

    #endregion

    //******************************************************************************\\

    #region Editor method

    TextAsset GetTexture(string _path)
    {
#if UNITY_EDITOR        
        TextAsset rt = new TextAsset();
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(_path.Replace(Application.dataPath, "Assets"));   
        TextureImporter texImport = (TextureImporter)TextureImporter.GetAtPath(_path.Replace(Application.dataPath, "Assets"));
        texImport.textureType = TextureImporterType.Default;
        texImport.alphaIsTransparency = false;
        texImport.anisoLevel = 1;
        texImport.borderMipmap = false;
        texImport.mipmapEnabled = false;
        texImport.textureCompression = TextureImporterCompression.Uncompressed;   
        EditorUtility.SetDirty(texImport);
        AssetDatabase.ImportAsset(_path.Replace(Application.dataPath, "Assets"));
        AssetDatabase.SaveAssets();

        string filePath = "";
        if (!m_isCustomFolder)
        {
            string pathInAsset = _path.Replace(Application.dataPath, "").Replace(tex.name + Path.GetExtension(_path), "").Replace("/", "_");
            if (!AssetDatabase.IsValidFolder(_folder_save_binary_tex_file_))
            {
                AssetDatabase.CreateFolder("Assets", _folder_save_binary_tex_file_.Replace("Assets/", ""));                                
            }
            if (!AssetDatabase.IsValidFolder(_folder_save_binary_tex_file_ + "/Save"))
            {
                AssetDatabase.CreateFolder(_folder_save_binary_tex_file_, "Save"); 
            }
            if (!AssetDatabase.IsValidFolder(_folder_save_binary_tex_file_ + "/Save/" + pathInAsset.Replace("Assets/", "")))
            {               
                AssetDatabase.CreateFolder(_folder_save_binary_tex_file_ + "/Save", pathInAsset.Replace("Assets/", ""));            
            }
            m_sPathTex = m_sPathTex.Replace(tex.name + Path.GetExtension(_path), "");
            filePath = _folder_save_binary_tex_file_ + "/Save/" + pathInAsset + "/" + tex.name + ".bytes";
        }
        else
        {
            filePath = m_sPathCustom.Replace(Application.dataPath, "Assets") + "/" + tex.name + ".bytes";
        }
        m_ListSizeTex[m_nCurrIndexTexProcess] = new Vector2(tex.width, tex.height);
        byte[] bytesTex = tex.EncodeToPNG();

        File.WriteAllBytes(Application.dataPath.Replace("Assets", "") + filePath, bytesTex); 
     
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();     
        rt = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
        m_listNameTextAssetTexture[m_nCurrIndexTexProcess] = tex.name;
        return rt;
#else
        return null;
#endif
    }

    #endregion

}
