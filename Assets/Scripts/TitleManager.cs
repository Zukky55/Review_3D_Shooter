using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Title
{
    public class TitleManager : MonoBehaviour
    {
        /// <summary>Image in Button</summary>
        [SerializeField] Image m_buttonImage;
        /// <summary>Text in Button</summary>
        [SerializeField] Text m_buttonText;
        /// <summary>Alpha color of Image and Text</summary>
        [Range(0, 1)] public float m_alpha;
        /// <summary>TitlePayer.cs</summary>
        TitlePlayer m_tp;
        /// <summary>Flag of change alpha color</summary>
        private bool m_alphaFlag = false;
        /// <summary>Time to change color</summary>
        private float m_changeTime = 3f;

        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            m_alphaFlag = true;
        }

        private void ChangingAlphaColor()
        {

            
        }

        private void Start()
        {
            m_tp = GameObject.Find("Player").GetComponent<TitlePlayer>();
            m_buttonImage = GameObject.Find("Button").GetComponent<Image>();
            m_buttonText = GameObject.Find("TextOfButton").GetComponent<Text>();
            m_buttonImage.color = new Color(0.5f, 0.5f, 0.5f, m_alpha);
            m_buttonText.color = new Color(1f, 1f, 1f, m_alpha);

            StartCoroutine(Wait(m_tp.waitTime));

        }
        private void Update()
        {
            if (m_alphaFlag)
            {
                if (m_alpha > 1f)
                {
                    m_alpha = 1f;
                    m_alphaFlag = false;
                }
                m_alpha += Time.deltaTime / m_changeTime;
                m_buttonImage.color = new Color(0.5f, 0.5f, 0.5f, m_alpha);
                m_buttonText.color = new Color(1f, 1f, 1f, m_alpha);
            }

        }
    }
}