using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextManager : MonoBehaviour
{
    /// <summary>会話文リスト</summary>
    [SerializeField][TextArea(1,4)] List<string> m_messamgeList = new List<string>();
    [SerializeField] Text m_text;
    [SerializeField] Image m_image;
    /// <summary>一文字一文字の表示する速さ</summary>
    [SerializeField] public float m_displaySpeed;
    /// <summary>現在表示中の会話文の配列</summary>
    private int m_displayListindex = 0;
    /// <summary>Alpha color of Image and Text</summary>
    [Range(0, 1)] public float m_alpha = 1f;

    private IEnumerator Display()
    {
        int messageCount = 0;//現在表示中の文字数
        m_text.color = new Color(1f, 1f, 1f, m_alpha);
        m_image.color = new Color(0.4f, 0.4f, 0.4f, m_alpha);

        m_text.text = ""; //reset text
        while (m_messamgeList[m_displayListindex].Length > messageCount) //文字を全て表示していない場合ループ
        {
            m_text.text += m_messamgeList[m_displayListindex][messageCount];
            messageCount++;//現在の文字数
            yield return new WaitForSeconds(m_displaySpeed); //任意の時間待つ
        }

        m_displayListindex++;
        if (m_displayListindex < m_messamgeList.Count)
        {
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            StartCoroutine(Display());
        }
        else
        {
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            m_alpha = 0f;
            m_text.color = new Color(1f, 1f, 1f, m_alpha);
            m_image.color = new Color(0.4f, 0.4f, 0.4f, m_alpha);
        }
    }


    private void Start()
    {
        m_alpha = 0f;
        m_text.color = new Color(1f, 1f, 1f, m_alpha);
        m_image.color = new Color(0.4f, 0.4f, 0.4f, m_alpha);

        StartCoroutine(Display());

    }
}
