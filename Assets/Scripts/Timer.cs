using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private int m_setMinute;
    /// <summary>The minute of the timer</summary>
    private int m_minute;
    [SerializeField] private float m_setSeconds;
    /// <summary>The second of the timer</summary>
    private float m_seconds;
    /// <summary>Seconds of the previous frame</summary>
    private float m_oldSeconds;
    /// <summary>Timer of SceneView</summary>
    private Text m_timerText;


    private void Initialize()
    {
        m_minute = m_setMinute;                                        //初期化
        m_seconds = m_setSeconds;
        m_oldSeconds = 0f;
        //m_timerText = GameObject.Find("Timer").GetComponent<Text>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if(GameManager.m_timerFlag)
        {
            m_seconds -= Time.deltaTime;
            if (m_seconds < 0f && m_minute > 0)
            {
                m_minute--;
                m_seconds = m_seconds + 60f;
            }
            if ((int)m_seconds != (int)m_oldSeconds)
            {
                //m_timerText.text = m_minute.ToString("00") + ":" + ((int)m_seconds).ToString("00");
            }
            m_oldSeconds = m_seconds;
            if (m_seconds <= 0f && m_minute <= 0)                  //timerが残り0秒になったらフラグ切ってtimerStop、ゲーム開始時や停止時に切り替える
            {
                GameManager.m_timerFlag = false;
                GameManager.m_timeUpCount = true;
            }
        }
    }



}
