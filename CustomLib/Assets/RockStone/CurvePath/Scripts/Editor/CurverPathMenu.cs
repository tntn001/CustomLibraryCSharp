using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CurverPathMenu
{
    [MenuItem("GameObject/Rock Stone/Curve Path",false,0)]
    static void CreateCurvePath()
    {
        GameObject go = new GameObject("Curver Path", typeof(RockStone.CurvePath));
        go.transform.parent = null;
        go.transform.position = Vector3.zero;
        Selection.activeObject =(Object)go;        
    }
}
