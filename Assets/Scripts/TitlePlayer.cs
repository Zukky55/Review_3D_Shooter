using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Title
{

    public class TitlePlayer : MonoBehaviour
    {
        /// <summary>Player's rigidbody</summary>
        private Rigidbody rb;
        /// <summary>Player's Turning speed</summary>
        [SerializeField] float m_spinSpeed;
        /// <summary>フレーム終了直前のposition</summary>
        private Vector3 m_offsetPosition;
        private bool m_isRunning = false;
        [Range(0, 1)] [SerializeField] internal float x = 0;
        internal bool flag = false;
        public float waitTime = 12.5f;

        void Update()
        {
            if (flag)
            {
                if (x < 1f)
                {
                    x += Time.deltaTime;
                }
                else
                {
                    x = 1f;
                }
            }
            else
            {
                if (x > 0f)
                {
                    x -= Time.deltaTime;
                }
                else
                {
                    x = 0f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.02f);
                }
            }
            var tilt = Quaternion.AngleAxis(-x * m_spinSpeed * Time.deltaTime, transform.forward);
            // 元の回転値と合成して上書き
            transform.rotation = tilt * transform.rotation;
        }


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                flag = true;
                yield return new WaitForSeconds(5f);
                flag = false;

            }
        }

        
    }
}



