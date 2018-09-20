using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    /// <summary>text of count down</summary>
    private Text m_text;

    /// <summary>Flag to accelerate when game starts</summary>
    public bool m_startAcceleration { get; private set; }
    /// <summary></summary>
    public Text m_scoreText { get; set; }
    private int m_score;

    private void Initialize()
    {
        m_startAcceleration = false;
        GameManager.m_startFlag = false;
        m_scoreText = GameObject.Find("Score").GetComponent<Text>();    //Display score in text
        m_text = GameObject.Find("Text").GetComponent<Text>();        //Display message in text
        m_text.text = "";
    }

    /// <summary>Display score to UI</summary>
    public void ShowScore()
    {
        m_scoreText.text = "Score:" + GameManager.m_scoreCount.ToString("00000000");
    }

    /// <summary>CountDown</summary>
    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(1f);                    //CountDown
        m_text.text = "THREE";
        yield return new WaitForSeconds(1f);
        m_text.text = "TWO";
        yield return new WaitForSeconds(1f);
        m_text.text = "ONE";
        yield return new WaitForSeconds(1f);
        m_text.text = "GO!!!";
        GameManager.m_startFlag = true;                                     //GameStart
        GameManager.m_timerFlag = true;                             //Start timer
        m_startAcceleration = true;                             //Start Accelerator
        yield return new WaitForSeconds(1f);
        m_text.text = "";
    }

    IEnumerator TimeUp()
    {
        m_text.text = "TimeUP!!!";
        yield return new WaitForSeconds(3f);
    }


    void Start()
    {
        FadeManager.FadeIn();
        Initialize();
        StartCoroutine(StartWait());
    }

    void Update()
    {
        if(GameManager.m_timeUpCount) //制限時間に達したら
        {

        }

    }

}