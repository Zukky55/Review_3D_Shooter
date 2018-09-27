using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    Attack=0,
    Drift,
    Straight
}
public class EnemyController : MonoBehaviour
{
    /// <summary>Enemy's state</summary>
    [SerializeField] private State m_state;
    /// <summary>Enemy's rigidbody</summary>
    Rigidbody m_rb;
    /// <summary>弾</summary>
    [SerializeField] public GameObject m_shot;
    /// <summary>重複呼び出し防止フラグ</summary>
    private bool m_isRunning = false;
    /// <summary>弾のスポーンポイント</summary>
    [SerializeField] public Transform m_shotSpawn;
    /// <summary>現在地から初期位置を引いた差分</summary>
    private Vector3 m_diff;
    /// <summary>Mother ship</summary>
    private GameObject m_motherShip;
    /// <summary>ステージ制限範囲を超えた時に旋回させる回転速度</summary>
    [SerializeField] float m_rotateTime;
    /// <summary>設定されたコライダーと接触した時のフラグ</summary>
    private bool m_boundaryFlag = false;
    /// <summary>ObjectController.cs</summary>
    private ObjectController m_oc;
    /// <summary>対象の回転</summary>
    private Quaternion m_targetRot;
    /// <summary>対象の座標</summary>
    private Vector3 m_targetPos;
    /// <summary>time count</summary>
    private float m_timeCount = 0f;
    [SerializeField] private float m_setTimer = 15f;
    /// <summary>MotherShipのセンター</summary>
    private Vector3 m_MSPos;
    /// <summary>対象を変えた回数</summary>
    [SerializeField] private int m_targetChangeCount = 0;
    /// <summary>対象を変える頻度</summary>
    [SerializeField] private float m_targetChangeRate = 5f;
    /// <summary>Set target change count.対象変更回数フラグ設定</summary>
    [SerializeField] private int m_setTCC = 3;
    [SerializeField] private float m_magnitude = 600f;
    private Animator m_animator;

    /// <summary>直進ステート</summary>
    private void GoingStraght()
    {
        m_rb.velocity = transform.forward * m_oc.m_myStatus.speed;  //常に機体の前方方向へ前進
        m_diff = m_MSPos - transform.position;  //Vector acquisition to initial position
        if (m_diff.magnitude > m_magnitude) //指定範囲を超えたら強制的に方向転換
        {
            m_targetRot = Quaternion.LookRotation(m_diff, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRot, Time.deltaTime / m_rotateTime);
        }

        if (m_boundaryFlag) //MotherSHipに近づきすぎたら遠ざける
        {
            m_diff = transform.position - m_MSPos; //MotherShipからみたenemy迄のvector
            m_targetRot = Quaternion.LookRotation(m_diff, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRot, Time.deltaTime / m_rotateTime);
        }
        m_timeCount += Time.deltaTime;
        if (m_timeCount > m_setTimer)    //設定した時間に達したらステート変更
        {
            m_timeCount = 0f;
            m_state = (State)Random.Range(0, 2);
        }
    }

    /// <summary>攻撃ステート</summary>
    private void Attacking()
    {
        m_rb.velocity = transform.forward * m_oc.m_myStatus.speed;  //常に機体の前方方向へ前進
        m_diff = m_MSPos - transform.position; //母船と自機の差分ベクトルを取り出す
        m_targetRot = Quaternion.LookRotation(m_diff, transform.up);
        if (transform.rotation != m_targetRot) //ターゲットの方を向く迄回転を加える
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRot, Time.deltaTime / m_rotateTime);
        }
        if(GameManager.m_startFlag)
        {
            StartCoroutine(WeaponShot());
        }
        m_timeCount += Time.deltaTime;

        if (m_boundaryFlag || m_timeCount > m_setTimer)
        {
            m_timeCount = 0f;
            m_state = (State)Random.Range(0, 2);
        }
    }

    /// <summary>浮遊ステート</summary>
    private void Drifting()
    {
        m_rb.velocity = transform.forward * m_oc.m_myStatus.speed;  //常に機体の前方方向へ前進
        if (m_targetChangeRate < m_timeCount)
        {
            m_diff = (m_MSPos + Random.onUnitSphere * 750f) - transform.position; //自分から見た、ランダムで取得したマザーシップからmagnitude750f離れた円の表面の座標への差分
            m_targetChangeCount++;
            m_timeCount = 0f;
        }
        m_targetRot = Quaternion.LookRotation(m_diff, transform.up); //ターゲットへ向く為の回転
        transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRot, Time.deltaTime / m_rotateTime); //Targetの方へ向き続ける
        m_timeCount += Time.deltaTime;
        if (m_boundaryFlag) //MotherSHipに近づきすぎたら遠ざける
        {
            m_diff = transform.position - m_MSPos; //MotherShipからみたenemy迄のvector
            m_targetRot = Quaternion.LookRotation(m_diff, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRot, Time.deltaTime / m_rotateTime);
        }
        if (m_targetChangeCount >= m_setTCC) //m_setTCC回対象変更を行ったら
        {
            m_timeCount = 0f;
            m_targetChangeCount = 0;
            m_state = (State)Random.Range(0, 2);
        }
    }

    private void OnTriggerEnter(Collider other)//MotherShipの境界線に入ったらフラグon
    {
        if (other.tag == "Boundary")
        {
            m_boundaryFlag = true;
        }
    }

    private void OnTriggerExit(Collider other)//MotherShipの境界線に入ったらフラグoff
    {
        if (other.tag == "Boundary")
        {
            m_boundaryFlag = false;
        }
    }

    /// <summary>ファイアーレート毎に弾を打つコルーチン</summary>
    IEnumerator WeaponShot()
    {
        if (m_isRunning)
        {
            yield break;
        }
        m_isRunning = true;
        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation);
        yield return new WaitForSeconds(m_oc.m_myStatus.fireRate);
        m_isRunning = false;
    }

    public Animator GetAnimator()
    {
        return m_animator;
    }

    private void Init()
    {
        m_rb = GetComponent<Rigidbody>();
        m_motherShip = GameObject.Find("MotherShip");
        m_rb.freezeRotation = true; //物理演算による回転の影響を無視させる
        m_oc = GetComponent<ObjectController>();
        m_state = State.Straight; //Stateの初期化
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if(m_motherShip != null)
        {
            m_MSPos = (m_motherShip.transform.position + (m_motherShip.transform.forward * GameManager.m_multiply));
        }
        switch (m_state)
        {
            case State.Attack:
                Attacking();
                break;
            case State.Drift:
                Drifting();
                break;
            case State.Straight:
                GoingStraght();
                break;
            default:
                break;
        }
    }
}