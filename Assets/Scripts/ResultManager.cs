using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    private Text m_titleText;
    private Text m_scoreText;
    private Text m_highScoreText;

    private void Show()
    {
        m_scoreText.text = "Score:" + GameManager.m_scoreCount.ToString("00000000");
        m_highScoreText.text = "HighScore:" + GameManager.m_highScore.ToString("00000000");
    }





    private void Start()
    {
        FadeManager.FadeIn();
        m_titleText = GameObject.Find("Title").GetComponent<Text>();
        m_scoreText = GameObject.Find("Title").GetComponent<Text>();
        m_highScoreText = GameObject.Find("Title").GetComponent<Text>();
    }

}
