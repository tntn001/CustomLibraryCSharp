using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public class TestBezier : MonoBehaviour
{
    public GameObject m_gtemplate;
    BezierCurve curve;

    private void Start()
    {
        curve = new BezierCurve(100, new Vector3(0, 0, 0), new Vector3(5, 2, 7), new Vector3(-10, -20, 5));
        for(int i =0; i < curve.Elements.Length; i++)
        {
            GameObject go = GameObject.Instantiate(m_gtemplate, null);
            go.transform.position = curve.Elements[i];
        }
    }
}
