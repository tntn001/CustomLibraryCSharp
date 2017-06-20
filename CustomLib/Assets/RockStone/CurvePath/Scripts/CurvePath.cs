using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockStone
{
    public class CurvePath : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private List<Vector3> m_ListElements;

        //[HideInInspector]
        [SerializeField]
        private List<CurvePathPoint> m_ListMainPoints;

        [HideInInspector]
        [SerializeField]
        private GameObject m_NewPointHasBeenCreated;

        [HideInInspector]
        [SerializeField]
        private bool m_isAround;

        private void OnValidate()
        {
            for (int i = 0; i < m_ListMainPoints.Count; i++)
            {
                m_ListMainPoints[i].OnDelete -= CallBackOnDeletePoint;
                m_ListMainPoints[i].OnDelete += CallBackOnDeletePoint;
            }
        }

        private void AddNewPoint()
        {
            CurvePathPoint curve = m_NewPointHasBeenCreated.GetComponent<CurvePathPoint>();
            curve.SetValue(m_ListMainPoints.Count, m_ListMainPoints[m_ListMainPoints.Count - 1]);
            m_ListMainPoints.Add(curve);
            m_ListMainPoints[m_ListMainPoints.Count - 1].OnDelete += CallBackOnDeletePoint;
            m_NewPointHasBeenCreated = null;
        }      

        private void CallBackOnDeletePoint(int index)
        {
            m_ListMainPoints.RemoveAt(index);
            Debug.Log("[CurvePath]: Rename main point!!!");
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
