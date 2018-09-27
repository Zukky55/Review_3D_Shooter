using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextManager : MonoBehaviour
{
    /// <summary>会話文リスト</summary>
    [SerializeField] [TextArea(1, 4)] List<string> m_list_start = new List<string>();
    [SerializeField] [TextArea(1, 4)] List<string> m_list_2 = new List<string>();
    [SerializeField] [TextArea(1, 4)] List<string> m_list_clear = new List<string>();
    [SerializeField] [TextArea(1, 4)] List<string> m_list_go = new List<string>();
    [SerializeField] [TextArea(1, 4)] List<string> m_list_after = new List<string>();
    /// <summary>条件によって代入するリストを変える変数</summary>
   　private List<string> m_displayList = new List<string>();
    [SerializeField] private Text m_text;
    [SerializeField] private Image m_image;
    [SerializeField] private float m_startWait;
    [SerializeField] private float m_displayWait = 1f;
    /// <summary>ゲームスタートのテキスト表示用</summary>
    [SerializeField] private Text m_gSText;
    /// <summary>一文字一文字の表示する速さ</summary>
    [SerializeField] public float m_displaySpeed;
    /// <summary>現在表示中の会話文の配列</summary>
    private int m_displayListIndex = 0;
    /// <summary>Alpha color of Image and Text</summary>
    [Range(0, 1)] public float m_alpha = 0f;
    /// <summary></summary>
    private bool m_firstToggle = true;
    private bool m_toggle2 = true;
    private bool m_toggle3 = true;
    private bool m_toggle4 = true;
    /// <summary>最初の一回だけ遅延を起こすフラグ</summary>
    private bool firstFlag = true;

    /// <summary>黒い背景</summary>
    private Image m_bImage;

    private IEnumerator Display()
    {
        m_alpha = 1f; //テキストとイメージのアルファ値を１にする
        int messageCount = 0;//現在表示中の文字数
        m_text.color = new Color(1f, 1f, 1f, m_alpha);
        m_image.color = new Color(0.4f, 0.4f, 0.4f, m_alpha);
        m_text.text = ""; //reset text

        if(firstFlag)
        {
            m_bImage.color = new Color(0f, 0f, 0f, 1f); //黒背景を表示
            yield return new WaitForSeconds(1f);
            firstFlag = false;
        }

        yield return new WaitForSeconds(m_startWait); //最初遅延させる

        while (m_displayList[m_displayListIndex].Length > messageCount) //文字を全て表示していない場合ループ
        {

            m_text.text += m_displayList[m_displayListIndex][messageCount];
            messageCount++;//現在の文字数
            yield return new WaitForSeconds(m_displaySpeed); //任意の時間待つ
            if(Input.GetMouseButton(0))
            {
                m_displaySpeed = 0.0001f;
            }
            else
            {
                m_displaySpeed = 0.1f;
            }
            //DEBUG
            if(Input.GetKey(KeyCode.P))
            {
                m_alpha = 0f;
                m_text.color = new Color(1f, 1f, 1f, m_alpha);
                m_image.color = new Color(0.4f, 0.4f, 0.4f, m_alpha);
                m_bImage.color = new Color(0f, 0f, 0f, 0f);
                if(!AudioManager.m_audio.isPlaying)
                {
                    AudioManager.StartAudio(); //Play BGM
                }
                m_displayListIndex = 0;
                GameManager.m_startFlag = true;                          //テキストを読み終えたらゲームスタート
                GameManager.m_NovelFlag = true;
                GameManager.m_timerFlag = true;
                StartCoroutine(DispWait());


                if (m_gSText != null && GameManager.m_startFlag)
                {
                    m_gSText.enabled = true;
                    yield return new WaitForSeconds(2f);
                    m_gSText.enabled = false;
                }
                yield break;
            }
        }

        m_displayListIndex++;
        if (m_displayListIndex < m_displayList.Count) // 次の文字列がある場合
        {
            yield return new WaitForSeconds(m_displayWait);
            if (m_displayListIndex == 3 && m_displayList == m_list_start) // 最初のテキストの時だけ存在している黒背景を消す
            {
                m_bImage.color = new Color(0f, 0f, 0f, 0f);　//黒背景を非表示にする
                AudioManager.StartAudio(); //Play BGM
            }
            StartCoroutine(Display());
        }
        else //全ての文字列を表示させたら
        {
            yield return new WaitForSeconds(m_displayWait);
            m_alpha = 0f;
            m_text.color = new Color(1f, 1f, 1f, m_alpha);
            m_image.color = new Color(0.4f, 0.4f, 0.4f, m_alpha);
            GameManager.m_startFlag = true;                             //テキストを読み終えたらゲームスタート
            GameManager.m_timerFlag = true;
            m_displayListIndex = 0;
            if (m_displayList == m_list_start)
            {
                StartCoroutine(DispWait());
            }
            if (gameObject.tag == "Novel") //"Novel"Sceneの時のみ行う挙動
            {
                GameManager.m_startFlag = false;
                GameManager.m_NovelFlag = true;
            }
            if(GameManager.m_afterFlag)//タイムアップまで持ちこたえた時のみ
            {
                FadeManager.FadeOut(2,2f); //resultSceneへ
            }

            if (GameManager.m_gameOverFlag)//gameOverの時のみ
            {
                GameManager.m_startFlag = false;
                FadeManager.FadeOut(2, 2f); //titleSceneヘ
            }
        }
    }

    IEnumerator DispWait()
    {
        m_gSText.enabled = true;
        m_gSText.fontSize = 0;
        while(m_gSText.fontSize < 100)
        {
            m_gSText.fontSize += 10;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        m_gSText.enabled = false;
    }
    private void Init()
    {
        if (gameObject.tag == "Novel")　//"Novel"Sceneの時のみ行う挙動
        {
            FadeManager.FadeIn();
        }
        if (m_list_start != null)
        {
            m_displayList = m_list_start; //初期のテキスト
        }

        m_firstToggle = true; //flag等初期値に全て戻す
        m_toggle2 = true;
        m_toggle3 = true;
        m_toggle4 = true;
        firstFlag = true;
        m_displayListIndex = 0;
        m_gSText.enabled = false;
        m_alpha = 0f;
        m_text.color = new Color(1f, 1f, 1f, m_alpha);
        m_image.color = new Color(0.4f, 0.4f, 0.4f, m_alpha);
        m_bImage = GameObject.Find("BImage").GetComponent<Image>();

    }

    private void Start()
    {
        Init();
        StartCoroutine(Display());
    }

    private void Update()
    {
        if(GameManager.m_NovelFlag && gameObject.tag == "Novel") //"Novel"Sceneの時のみ行う挙動
        {
            FadeManager.FadeOut(2, 2f);
        }

        if (MainManager.m_totalSeconds < 20 && MainManager.m_totalSeconds > 0)//残り秒数によって代入するテキストを変える
        {
            if(m_firstToggle)
            {
                m_displayListIndex = 0;
                m_displayList = m_list_2;
                StartCoroutine(Display());
                m_firstToggle = false;
            }
        }
        if (GameManager.m_clearFlag)
        {
            if (m_toggle2)
            {
                m_displayListIndex = 0;
                GameManager.m_startFlag = false;
                m_displayList = m_list_clear;
                StartCoroutine(Display());
                m_toggle2 = false;
            }
        }
        if (GameManager.m_gameOverFlag)
        {
            if (m_toggle3)
            {
                m_displayListIndex = 0;
                m_displayList = m_list_go;
                StartCoroutine(Display());
                m_toggle3 = false;
            }
        }        if (GameManager.m_afterFlag)
        {
            if (m_toggle4)
            {
                m_displayListIndex = 0;
                m_displayList = m_list_after;
                StartCoroutine(Display());
                m_toggle4 = false;
            }
        }
    }
}
