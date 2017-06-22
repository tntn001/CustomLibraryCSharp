using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockStone.CustomMath;

namespace RockStone
{
    public class CurvePath : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private List<Vector3> m_ListElements;

        [HideInInspector]
        [SerializeField]
        private List<CurvePathPoint> m_ListMainPoints;

        //[HideInInspector]
        [SerializeField]
        private List<CurvePathPoint> m_ListCenterPoints;

        [HideInInspector]
        [SerializeField]
        private GameObject m_NewPointHasBeenCreated;

        [HideInInspector]
        [SerializeField]
        private bool m_isAround;

        [SerializeField]
        private Color m_Color = Color.red;
        [SerializeField]
        private Color m_ColorSubPoint = Color.green;

        [SerializeField]
        private float m_Size = 1;

        [SerializeField]
        private float m_SizeSubPoint = 1;

        private void OnValidate()
        {
            for (int i = 0; i < m_ListMainPoints.Count; i++)
            {
                m_ListMainPoints[i].OnDelete -= CallBackOnDeletePoint;
                m_ListMainPoints[i].OnDelete += CallBackOnDeletePoint;
                m_ListMainPoints[i].PointColor = m_Color;
                m_ListMainPoints[i].PointSize = m_Size;
            }          
        }        

        private void AddNewPoint()
        {
            CurvePathPoint curve = m_NewPointHasBeenCreated.GetComponent<CurvePathPoint>();
            curve.SetValue(m_ListMainPoints.Count);
            curve.PointColor = m_Color;
            curve.PointSize = m_Size;
            m_ListMainPoints.Add(curve);
            m_ListMainPoints[m_ListMainPoints.Count - 1].OnDelete += CallBackOnDeletePoint;
            m_NewPointHasBeenCreated = null;
            if(m_ListMainPoints.Count > 1)
            {
                GameObject go = new GameObject("Center");                
                CurvePathPoint center =  go.AddComponent<CurvePathPoint>();
                center.transform.SetParent(m_ListMainPoints[m_ListMainPoints.Count - 2].transform);
                center.PointColor = m_ColorSubPoint;
                center.PointSize = m_SizeSubPoint;
                m_ListCenterPoints.Add(center);
            }
        }      

        private void CallBackOnDeletePoint(int index)
        {
            m_ListMainPoints.RemoveAt(index);

            if (m_ListCenterPoints.Count > 0)
            {
                DestroyImmediate(m_ListCenterPoints[index].gameObject);
                m_ListCenterPoints.RemoveAt(index);
            }

            for (int i = 0; i < m_ListMainPoints.Count; i++)
            {
                m_ListMainPoints[i].OnDelete -= CallBackOnDeletePoint;
                m_ListMainPoints[i].name = "Point " + (i + 1);
                m_ListMainPoints[i].SetValue(i);
                m_ListMainPoints[i].OnDelete += CallBackOnDeletePoint;
            }
            ReloadCurve();
        }

        private void ReloadCurve()
        {

        }
    }
}
