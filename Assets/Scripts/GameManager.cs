using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary></summary>
public class GameManager : MonoBehaviour
{
    /// <summary>Keep score</summary>
    public static int m_scoreCount { get; private set; }
    /// <summary>Most high score of game</summary>
    public static int m_highScore { get; private set; }
    /// <summary>Flag switch</summary>
    public static bool m_timerFlag = false;
    public static bool m_timeUpCount = false;
    /// <summary>If Game starts</summary>
    public static bool m_startFlag = false;
    private static string m_highScoreKey = "highScore";
    
    /// <summary>Initialize</summary>
    public static void Init()
    {
        m_scoreCount = 0;
        m_highScore = PlayerPrefs.GetInt(m_highScoreKey, 0);                    //m_highScoreを取得。保存されていなければ0で初期化
    }
    
    /// <summary>Add score of destroyed enemies</summary>
    /// <param name="score">Score of destroyed object</param>
    public static void AddScore(int score)
    {
        m_scoreCount += score;
        if(m_highScore < m_scoreCount)
        {
            m_highScore = m_scoreCount;
        }
    }

    /// <summary>Save high score</summary>
    public void Save()
    {
        PlayerPrefs.SetInt(m_highScoreKey, m_highScore);
        PlayerPrefs.Save();

        Init();                                                                 //Return before game starts
    }
}
