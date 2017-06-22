using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RockStone
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class CurvePathPoint : MonoBehaviour
    {
        public event UnityAction<int> OnDelete;

        [HideInInspector]
        [SerializeField]
        private int m_nCurrentIndex;     

        private Color m_color;
        public Color PointColor
        {
            get { return m_color; }
            set { m_color = value; }
        }

        private float m_size;
        public float PointSize
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public void SetValue(int index)
        {
            m_nCurrentIndex = index;
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
        
        private void OnDrawGizmos()
        {
            Gizmos.color = m_color;
            Gizmos.DrawSphere(transform.position, m_size);            
        }
    }
#endif
}
