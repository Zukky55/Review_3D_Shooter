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
    public static Text m_scoreText { get; set; }
    private int m_score;
    /// <summary>ゲームの制限時間を秒数変換した変数</summary>
    public static float m_totalSeconds;
    /// <summary>シーン上のアステロイド</summary>
    private static GameObject[] m_asteroids;
    /// <summary>シーン上のエネミー</summary>
    private static GameObject[] m_enemies;
    private static AudioSource m_audioS;
    /// <summary>一回だけ呼び出すためのフラグ</summary>
    private bool m_flag = true;




    private void Initialize()
    {
        m_startAcceleration = false;
        GameManager.m_startFlag = false;
        m_scoreText = GameObject.Find("Score").GetComponent<Text>();    //Display score in text
        m_text = GameObject.Find("Text").GetComponent<Text>();        //Display message in text
        m_text.text = "";
        m_audioS = GetComponent<AudioSource>();
        m_flag = true;
    }

    /// <summary>Display score to UI</summary>
    public static void ShowScore()
    {
        m_scoreText.text = "Score:" + GameManager.m_scoreCount.ToString("00000000");
    }

    /// <summary>scene上の敵オブジェクトを全て破壊する</summary>
    public static void DestroyAll()
    {
        m_asteroids = GameObject.FindGameObjectsWithTag("Asteroid"); //常にscene上にいる小惑星と敵の数を取得、」クリアーフラグが立ったら全て一斉破壊、爆破音もここで流す
        m_enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var item in m_asteroids)
        {
            Destroy(item);
        }
        foreach (var item in m_enemies)
        {
            Destroy(item);
        }
        m_audioS.Play();
    }



    void Start()
    {
        FadeManager.FadeIn();
        Initialize();
    }

    private void Update()
    {
        if(GameManager.m_clearFlag && m_flag)//クリアしたら敵全破壊＆爆破音声再生
        {
            //DestroyAll();
            AudioManager.StopBgm();
            m_flag = false;
        }
        Debug.Log(GameManager.m_startFlag);
    }
}