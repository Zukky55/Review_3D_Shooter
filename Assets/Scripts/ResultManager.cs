using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    /// <summary>Score</summary>
    private Text m_scoreText;
    /// <summary>HighScore</summary>
    private Text m_highScoreText;
    /// <summary>Image in Button</summary>
    private Image m_buttonImage;
    /// <summary>Text in Button</summary>
    private Text m_buttonText;
    /// <summary>Title</summary>
    private Text m_title;
    /// <summary>Alpha color of Text</summary>
    [Range(0, 1)] public float m_scoreAlpha = 0f;
    /// <summary>Alpha color of Image and Text</summary>
    [Range(0, 1)] public float m_buttonAlpha = 0f;
    /// <summary>Alpha color of Title</summary>
    [Range(0, 1)] public float m_titleAlpha = 0f;
    /// <summary>Flag of change alpha color of score</summary>
    private bool m_scoreAlphaFlag = false;
    /// <summary>Flag of change alpha color of button</summary>
    private bool m_buttonAlphaFlag = false;
    /// <summary>Flag of change alpha color of title</summary>
    private bool m_titleAlphaFlag = false;
    /// <summary>Time to change color</summary>
    private float m_changeTime = 3f;

    IEnumerator Show()
    {
        m_scoreText.text = "Score:" + GameManager.m_scoreCount.ToString("00000000");
        m_highScoreText.text = "HighScore:" + GameManager.m_highScore.ToString("00000000");
        m_titleAlphaFlag = true;
        yield return new WaitForSeconds(2f);
        m_scoreAlphaFlag = true;
        yield return new WaitForSeconds(2f);
        m_buttonAlphaFlag = true;

    }

    private void Start()
    {
        FadeManager.FadeIn();
        m_title = GameObject.Find("Title").GetComponent<Text>();
        m_title.color = new Color(1f, 1f, 1f, m_titleAlpha);
        m_scoreText = GameObject.Find("Score").GetComponent<Text>();
        m_highScoreText = GameObject.Find("HighScore").GetComponent<Text>();
        m_scoreText.color = new Color(1f, 1f, 1f, m_scoreAlpha);
        m_highScoreText.color = new Color(1f, 1f, 1f, m_scoreAlpha);
        m_buttonImage = GameObject.Find("ResultButton").GetComponent<Image>();
        m_buttonText = GameObject.Find("TextOfButton").GetComponent<Text>();
        m_buttonImage.color = new Color(0.5f, 0.5f, 0.5f, m_buttonAlpha);
        m_buttonText.color = new Color(1f, 1f, 1f, m_buttonAlpha);

        StartCoroutine(Show());
        GameManager.Save();
    }

    private void Update()
    {
        if (m_scoreAlphaFlag)
        {
            if (m_scoreAlpha > 1f)
            {
                m_scoreAlpha = 1f;
                m_scoreAlphaFlag = false;
            }
            m_scoreAlpha += Time.deltaTime / m_changeTime;
            m_scoreText.color = new Color(1f, 1f, 1f, m_scoreAlpha);
            m_highScoreText.color = new Color(1f, 1f, 1f, m_scoreAlpha);
        }

        if (m_buttonAlphaFlag)
        {
            if (m_buttonAlpha > 1f)
            {
                m_buttonAlpha = 1f;
                m_buttonAlphaFlag = false;
            }
            m_buttonAlpha += Time.deltaTime / m_changeTime;
            m_buttonImage.color = new Color(0.5f, 0.5f, 0.5f, m_buttonAlpha);
            m_buttonText.color = new Color(1f, 1f, 1f, m_buttonAlpha);
        }
        if (m_titleAlphaFlag)
        {
            if (m_titleAlpha > 1f)
            {
                m_titleAlpha = 1f;
                m_titleAlphaFlag = false;
            }
            m_titleAlpha += Time.deltaTime / m_changeTime;
            m_title.color = new Color(1f, 1f, 1f, m_titleAlpha);
        }

    }

}
