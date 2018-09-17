using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    /// <summary>The minute hand of the timer</summary>
    public int m_setMinute
    {
        get { return m_minute; }
        set { m_minute = value; }
    }
    [SerializeField] private int m_minute;
    /// <summary>The second hand of the timer</summary>
    public float m_setSeconds
    {
        get { return m_seconds; }
        set { m_seconds = value; }
    }
    [SerializeField] private float m_seconds;
    /// <summary>Seconds of the previous frame</summary>
    private float m_oldSeconds;
    /// <summary>Timer of SceneView</summary>
    private Text m_timerText;
    /// <summary>Flag at the end of timer</summary>
    private bool m_timerStopFlag = true;


    private void Initialize()
    {
        //m_minute = 1;
        //m_seconds = 0f;
        m_oldSeconds = 0f;
        m_timerText = GameObject.Find("Timer").GetComponent<Text>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if(m_timerStopFlag)
        {
            m_seconds -= Time.deltaTime;
            if (m_seconds < 0f && m_minute > 0)
            {
                m_minute--;
                m_seconds = m_seconds + 60f;
            }
            if ((int)m_seconds != (int)m_oldSeconds)
            {
                m_timerText.text = m_minute.ToString("00") + ":" + ((int)m_seconds).ToString("00");
            }
            m_oldSeconds = m_seconds;
            if (m_seconds <= 0f && m_minute <= 0)                  //timerが残り0秒になったらフラグ切ってtimerStop、ゲーム開始時や停止時に切り替える
            {
                m_timerStopFlag = false;
            }
        }
    }



}
