using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RockStone
{
    [ExecuteInEditMode]
    public class CurvePathPoint : MonoBehaviour
    {
        public event UnityAction<int> OnDelete;

        [HideInInspector]
        [SerializeField]
        private int m_nCurrentIndex;

        //[HideInInspector]
        [SerializeField]
        private CurvePathPoint m_CurvePrevious;
        //[HideInInspector]
        [SerializeField]
        private CurvePathPoint m_CurveNext;

        public void SetValue(int index, CurvePathPoint previous = null, CurvePathPoint next = null)
        {
            m_nCurrentIndex = index;
            m_CurvePrevious = previous;
            m_CurveNext = next;
        }

        private void OnDestroy()
        {
            if(OnDelete != null)
            {
                OnDelete(m_nCurrentIndex);
            }
        }

        private void Start()
        {
            if(Application.isEditor && !Application.isPlaying)
            {
                
                
            }
        }
    }
}
