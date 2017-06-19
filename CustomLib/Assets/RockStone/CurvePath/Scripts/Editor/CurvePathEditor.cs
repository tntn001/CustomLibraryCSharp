using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RockStone
{
    [CustomEditor(typeof(CurvePath))]
    public class CurvePathEditor : Editor
    {
        SerializedProperty ListElements;
        SerializedProperty ListMainPoints;
        SerializedProperty NewPointHasBeenCreated;
        SerializedProperty IsAround;
        CurvePath m_Main;

        private void OnEnable()
        {
            m_Main = (CurvePath)target;
            ListElements = serializedObject.FindProperty("m_ListElements");
            ListMainPoints = serializedObject.FindProperty("m_ListMainPoints");
            NewPointHasBeenCreated = serializedObject.FindProperty("m_NewPointHasBeenCreated");
            IsAround = serializedObject.FindProperty("m_isAround");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IsAround.boolValue = GUILayout.Toggle(IsAround.boolValue, "Is Around");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("New point"))
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.name = "Point " + (ListMainPoints.arraySize + 1);
                obj.AddComponent<CurvePathPoint>();
                DestroyImmediate(obj.GetComponent<Collider>());
                obj.transform.parent = m_Main.transform;
                NewPointHasBeenCreated.objectReferenceValue = (Object)obj;
                m_Main.Invoke("AddNewPoint", 0);
            }
            if (GUILayout.Button("Clear All"))
            {

            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

        }

    }
}

