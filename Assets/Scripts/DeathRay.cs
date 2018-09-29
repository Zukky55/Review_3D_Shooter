using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRay : MonoBehaviour
{
    /// <summary>AudioSource</summary>
    private AudioSource m_audioS;
    /// <summary>レーザーの音</summary>
    [SerializeField] AudioClip m_deathRaySE;
    /// <summary>DeathRay</summary>
    [SerializeField] public GameObject m_deathRay;
    /// <summary>DeathRay ParticleSystem</summary>
    private ParticleSystem m_drps;
    /// <summary>Of m_drps</summary>
    private ParticleSystem.MainModule m_DR_main;
    /// <summary>MinMaxCurve variable "Min"</summary>
    private float m_min;
    /// <summary>MinMaxCurve variable "Max"</summary>
    private float m_max;
    private bool m_ParticleStartFlag = true;
    /// <summary>任意の数値(m_targetValue) / 到達させたい時間(Manager.m_totalSeconds)の計算結果</summary>
    private float m_calculationResult;
    /// <summary>ParticleSizeの目標値</summary>
    [SerializeField] private float m_targetPaticlerSize = 1.0f;
    /// <summary>Fire()を一回だけ呼ぶ為のフラグ</summary>
    private bool m_isAlreadyCalled = false;
    /// <summary>ObjectController.cs</summary>
    private ObjectController m_oc;
    /// <summary>explosion of enemy</summary>
    [SerializeField] GameObject m_eEnemy;
    /// <summary>explosion of asteroid</summary>
    [SerializeField] GameObject m_eAsteroid;

    private GameObject[] m_asteroids;
    private GameObject[] m_enemies;
    /// <summary>All Targets</summary>
    private List<GameObject> m_targets = new List<GameObject>();


    private void Growing()
    {
        if (GameManager.m_startFlag)
        {
            if (m_ParticleStartFlag) //Call it only one
            {
                m_drps.Play(); //Play ParticleSystem
                m_ParticleStartFlag = false;
            }
            //設定した時間に任意の数値に到達させるには、任意の数値(m_targetValue) / 到達させたい時間(Manager.m_totalSeconds) * フレーム毎の経過秒数(Time.deltaTime)
            m_min = m_max += m_calculationResult * Time.deltaTime;
            m_DR_main.startSize = new ParticleSystem.MinMaxCurve(m_min, m_max);
        }
        if(GameManager.m_clearFlag) //クリアしたら、滑らかにパーティクルを止める
        {
            m_min = m_max -= Time.deltaTime;
            m_DR_main.startSize = new ParticleSystem.MinMaxCurve(m_min, m_max);
            if(m_min < 0f || m_max < 0f)
            {
                m_min = m_max = 0f;
                m_drps.Stop();
            }
        }
    }

    /// <summary>全ての敵オブジェクト情報を返す</summary>
    /// <returns>敵オブジェクト全て入ったリスト</returns>
    private void GetTarget()
    {

        m_asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        m_enemies = GameObject.FindGameObjectsWithTag("Enemy");


        foreach (var obj in m_asteroids)
        {
            m_targets.Add(obj);

        }
        foreach (var obj in m_enemies)
        {
            m_targets.Add(obj);
        }
    }

    IEnumerator ShotEach()
    {
        GetTarget(); //今いる敵オブジェクトを全て取得
        if(m_targets.Count > 0)
        {
            m_audioS.PlayOneShot(m_deathRaySE); //Play SE
            foreach (var obj in m_targets)
            {
                var test = (Instantiate(m_deathRay, transform.position, transform.rotation) as GameObject).GetComponent<FireDeathRay>();
                test.m_target = obj;
                yield return null;
            }
            yield return new WaitForSeconds(4f);
            foreach (var obj in m_targets) //もれた敵を破壊
            {
                if (obj != null)
                {
                    m_oc = obj.GetComponent<ObjectController>();
                    switch (m_oc.m_type)
                    {
                        case Type.Asteroid:
                            Destroy(Instantiate(m_eAsteroid, obj.transform.position, obj.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                            Destroy(obj.transform.gameObject);
                            break;
                        case Type.WeakEnemy:
                        case Type.NormalEnemy:
                        case Type.StrongEnemy:
                        case Type.Planet:
                            Destroy(Instantiate(m_eEnemy, obj.transform.position, obj.transform.rotation), m_eEnemy.GetComponent<ParticleSystem>().main.duration);
                            Destroy(obj.transform.gameObject);
                            break;
                        case Type.MotherShip:
                        case Type.Other:
                        case Type.Player:
                        default:
                            break;
                    }
                }
            }
        }
        GameManager.m_afterFlag = true;
    }
    
    //IEnumerator ShotEach()
    //{
    //    GetTarget(); //今いる敵オブジェクトを全て取得
    //    m_audioS.PlayOneShot(m_deathRaySE); //Play SE
    //    while (m_targets.Count > 0)
    //    {
    //        Debug.Log("呼ばれました");
    //        foreach (var obj in m_targets)
    //        {
    //            var test = (Instantiate(m_deathRay, transform.position, transform.rotation) as GameObject).GetComponent<FireDathRay>();
    //            test.m_target = obj;
    //            yield return null;
    //        }
    //        GetTarget(); //今いる敵オブジェクトを全て取得
    //    }
    //    GameManager.m_afterFlag = true;
    //}

    private void Fire()
    {
        GetTarget(); //今いる敵オブジェクトを全て取得
        m_audioS.PlayOneShot(m_deathRaySE); //Play SE
        foreach (var obj in m_targets)
        {
            var test = (Instantiate(m_deathRay, transform.position, transform.rotation) as GameObject).GetComponent<FireDeathRay>();

            test.m_target = obj;
        }

    }

    /// <summary>触れたら敵オブジェクト破壊</summary>
    /// <param name="obj">enemy object</param>



    private void Init()
    {
        m_ParticleStartFlag = true;
        m_drps = GetComponent<ParticleSystem>();
        m_DR_main = m_drps.main;
        m_min = m_max = 0f; //Initialize of Min and Max variables
        m_DR_main.startSize = new ParticleSystem.MinMaxCurve(m_min, m_max); //The initial value is 0 for both min and max
        m_calculationResult = m_targetPaticlerSize / MainManager.m_totalSeconds; //目標値 / 到達させたい時間
        m_audioS = GetComponent<AudioSource>();

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Growing();
        if (GameManager.m_clearFlag && !m_isAlreadyCalled)
        {
            //Fire();
            StartCoroutine(ShotEach());
            m_isAlreadyCalled = true;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Fire();
            //StartCoroutine(ShotEach());
        }
    }
}
