using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>マップの境界</summary>
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}


public class MainManager : MonoBehaviour
{
    /// <summary>text of count down</summary>
    private Text m_text;
    /// <summary>player's game object</summary>
    private GameObject m_player;
    /// <summary>playerController.cs</summary>
    private PlayerController m_pc;
    /// <summary>If Game starts</summary>
    public bool m_startFlag { get; private set; }
    /// <summary>Flag to accelerate when game starts</summary>
    public bool m_startAcceleration { get; private set; }

    private void Initialize()
    {
        m_startAcceleration = false;
        m_startFlag = false;
        m_player = GameObject.Find("Player");                         //Get Player's component
        m_pc = m_player.GetComponent<PlayerController>();
        m_text = GameObject.Find("Text").GetComponent<Text>();        //Display message in text
        m_text.text = "";
        m_pc.m_setMoveSpeed = 0f;
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
        m_startFlag = true;                                     //GameStart
        m_startAcceleration = true;
        yield return new WaitForSeconds(1f);
        m_text.text = "";
    }

    void Start()
    {
        FadeManager.FadeIn();
        Initialize();
        StartCoroutine(StartWait());
    }


    void Update()
    {
        if(m_startFlag)
        {
            if (m_startAcceleration)                            //プレイヤーのスピードが50になる迄加速する
            {
                if (m_pc.m_setMoveSpeed > 50f)
                {
                    m_startAcceleration = false;
                }
                m_pc.m_setMoveSpeed += 1f;
            }
        }
    }

}
