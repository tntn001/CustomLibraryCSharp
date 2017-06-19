/*
 * CREATED BY MONKEY
 * 16-06-2017 (DD-MM-YYYY)
 * Mail: ti.en.ti0000@gmail.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockStone.CustomMath
{
    public struct BezierCurve
    {
        #region PRIVATE FIELDS
        private Vector3 m_v3MainPoint01;
        private Vector3 m_v3MainPoint02;
        private Vector3 m_v3MainPoint03;
        private Vector3 m_v3MainPoint04;
        private Vector3[] m_arrElement;
        private int m_nNumberOfElement;
        private int m_nTypeOfBezier;
        #endregion PRIVATE FIELDS

        #region PROPERTIES
        public Vector3[] Elements
        {
            get { return m_arrElement; }
        }
        #endregion PROPERTIES

        #region INIT
        /// <summary>
        /// Initialization quadratic Bezier curve 
        /// </summary>
        /// <param name="numberOfElement"> The number of element point </param>
        /// <param name="p1"> Point 1</param>
        /// <param name="p2"> Point 2</param>
        /// <param name="p3"> Point 3</param>
        public BezierCurve(int numberOfElement,Vector3 p1, Vector3 p2, Vector3 p3)
        {
            m_arrElement = new Vector3[numberOfElement];
            m_v3MainPoint01 = p1;
            m_v3MainPoint02 = p2;
            m_v3MainPoint03 = p3;
            m_v3MainPoint04 = p3;
            m_nNumberOfElement = numberOfElement;
            m_nTypeOfBezier = 2;
            CreateElements();           
        }

        /// <summary>
        /// Initialization cubic Bezier curve
        /// </summary>
        /// <param name="numberOfElement"> The number of element point </param>
        /// <param name="p1"> Point 1</param>
        /// <param name="p2"> Point 2</param>
        /// <param name="p3"> Point 3</param>
        /// <param name="p4"> Point 4</param>
        public BezierCurve(int numberOfElement, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            m_arrElement = new Vector3[numberOfElement];
            m_v3MainPoint01 = p1;
            m_v3MainPoint02 = p2;
            m_v3MainPoint03 = p3;
            m_v3MainPoint04 = p4;
            m_nNumberOfElement = numberOfElement;
            m_nTypeOfBezier = 3;
            CreateElements();
        }
        #endregion INIT

        #region PRIVATE METHOD
        private void CreateElements()
        {
            Vector3 element = Vector3.zero;
            float t = 0;
            float _1_t = 0;
            if (m_nTypeOfBezier == 2)
            {
                for (int i = 0; i < m_nNumberOfElement; i++)
                {
                    t = (i * 1f) / (m_nNumberOfElement * 1f);
                    _1_t = 1 - t;
                    element.x = _1_t * (_1_t * m_v3MainPoint01.x + t * m_v3MainPoint02.x) + t * (_1_t * m_v3MainPoint02.x + t * m_v3MainPoint03.x);
                    element.y = _1_t * (_1_t * m_v3MainPoint01.y + t * m_v3MainPoint02.y) + t * (_1_t * m_v3MainPoint02.y + t * m_v3MainPoint03.y);
                    element.z = _1_t * (_1_t * m_v3MainPoint01.z + t * m_v3MainPoint02.z) + t * (_1_t * m_v3MainPoint02.z + t * m_v3MainPoint03.z);
                    m_arrElement[i] = element;
                }
            }
            else if(m_nTypeOfBezier == 3)
            {

            }
        }
        #endregion PRIVATE METHOD

        #region PUBLIC METHOD

        /// <summary>
        /// Change value of bezier curve
        /// </summary>
        /// <param name="numberOfElement"> The number of element point </param>
        /// <param name="p1"> Point 1</param>
        /// <param name="p2"> Point 2</param>
        /// <param name="p3"> Point 3</param>
        public void ChangeValue(int numberOfElement, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            m_arrElement = null;
            m_arrElement = new Vector3[numberOfElement];
            m_v3MainPoint01 = p1;
            m_v3MainPoint02 = p2;
            m_v3MainPoint03 = p3;
            m_nNumberOfElement = numberOfElement;
            m_nTypeOfBezier = 2;
            CreateElements();            
        }

        /// <summary>
        /// Change value of bezier curve
        /// </summary>
        /// <param name="numberOfElement"> The number of element point </param>
        /// <param name="p1"> Point 1</param>
        /// <param name="p2"> Point 2</param>
        /// <param name="p3"> Point 3</param>
        /// <param name="p4"> Point 4</param>
        public void ChangeValue(int numberOfElement, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            m_arrElement = null;
            m_arrElement = new Vector3[numberOfElement];
            m_v3MainPoint01 = p1;
            m_v3MainPoint02 = p2;
            m_v3MainPoint03 = p3;
            m_v3MainPoint04 = p4;
            m_nNumberOfElement = numberOfElement;
            m_nTypeOfBezier = 3;
            CreateElements();
        }

        /// <summary>
        /// Check if point is near what element by delta distance.
        /// <para>Return index of element array.</para>
        /// </summary>
        /// <param name="point">Point need to check.</param>
        /// <param name="delta">Delta distance.</param>
        /// <returns></returns>
        public int CheckCollision(Vector3 point, float delta)
        {
            int _return = -1;
            float minDistance = delta;
            float distance = -1;
            for(int i = 0; i < m_arrElement.Length; i++)
            {
                distance = Vector3.Distance(point, m_arrElement[i]);
                if(distance <= Mathf.Epsilon)
                {
                    return i;
                }
                if (distance <= delta)
                {
                     if(minDistance == delta || distance < minDistance)
                    {
                        minDistance = distance;
                        _return = i;
                    }
                }
            }
            return _return;
        }

        #endregion
    }
}
