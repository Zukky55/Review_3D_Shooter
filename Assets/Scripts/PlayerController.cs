using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    /// <summary>Player's rigidbody</summary>
    Rigidbody m_rb;
    /// <summary>弾</summary>
    [SerializeField]  public GameObject m_shot;
    /// <summary>弾のスポーンポイント</summary>
    [SerializeField] public Transform m_shotSpawn;
    /// <summary>弾のファイヤーレート</summary>
    [SerializeField] float m_fireRate;
    /// <summary>弾の音</summary>
    AudioSource m_bs;
    /// <summary>Player's speed ()</summary>
    [SerializeField] private float m_moveSpeed;
    public float m_setMoveSpeed
    {
        get { return m_moveSpeed; }
        set { m_moveSpeed = value; }
    }
    /// <summary>Player's Turning speed</summary>
    [SerializeField] float m_turningSpeed;
    /// <summary>重複呼び出し防止フラグ</summary>
    private bool m_isRunning = false;
    /// <summary>MainManager.cs</summary>
    private MainManager m_mm;
    ParticleSystem m_rightAfterBurner;
    ParticleSystem m_leftAfterBurner;
    /// <summary>ステージ開始時にパーティクルを再生するフラグ</summary>
    private bool m_particleFlag = true;
    /// <summary>GameStart時の初期位置</summary>
    private Vector3 m_initPos;
    /// <summary>現在地から初期位置を引いた差分</summary>
    private Vector3 m_diff;
    /// <summary>ステージ制限範囲を超えた時に旋回させる回転速度</summary>
    [SerializeField] float m_rotateTime;
    /// <summary>Input acceleration</summary>
    private float m_inputAcceleration;




    private void Initialize()
    {
        m_rb = GetComponent<Rigidbody>();
        m_mm = GameObject.Find("MainManager").GetComponent<MainManager>();
        m_rightAfterBurner = GameObject.Find("RightAfterburner").GetComponent<ParticleSystem>();
        m_leftAfterBurner = GameObject.Find("LeftAfterburner").GetComponent<ParticleSystem>();
        m_initPos = transform.position;                         //Save initial position

    }

    /// <summary>機体を自動的に前進させる</summary>
    void Move()
    {
        m_rb.velocity = transform.forward * m_moveSpeed;
    }

    void Turn ()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        var tilt = Quaternion.AngleAxis(-x * m_turningSpeed * Time.deltaTime, transform.forward);
        //Quaternion yAxisRot = Quaternion.AngleAxis(x * m_turnAcceleration * Time.deltaTime, Vector3.up);
        Quaternion xAxisRot = Quaternion.AngleAxis(y * m_turningSpeed * Time.deltaTime, transform.right);
        // 元の回転値と合成して上書き
        transform.rotation =tilt * xAxisRot * transform.rotation;        
    }

    IEnumerator WeaponShot()
    {
        if (m_isRunning)
        {
            yield break;
        }
        m_isRunning = true;

        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation);
        m_bs = GetComponent<AudioSource>();
        m_bs.Play();
        yield return new WaitForSeconds(m_fireRate);
        m_isRunning = false;
    }


    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(WeaponShot());
        }
    }

    private void FixedUpdate()
    {
        var rightMa = m_rightAfterBurner.main;                  //ParticleSystem.main of RightAfterBurner
        var leftMa = m_leftAfterBurner.main;                    //ParticleSystem.main of LeftAfterBurner

        //If the game starts
        if (m_mm.m_startFlag)
        {
            Move();                                     //Method to move forward
            Turn();                                     //Method to turning player
            if (m_particleFlag)
            {
                m_rightAfterBurner.Play();              //Play particle system
                m_leftAfterBurner.Play();
                m_particleFlag = false;
            }
            m_diff = m_initPos - transform.position;    //Vector acquisition to initial position
            if (m_diff.magnitude > 500f)                //指定範囲を超えたら強制的に方向転換
            {
                var targetRotation = Quaternion.LookRotation(m_diff, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotateTime);
            }

            m_inputAcceleration = Input.GetAxis("Acceleration");                    //Inputを自分でカスタマイズしてSpaceで減速,Shiftで加速にしている
            if (m_inputAcceleration < 0f && m_moveSpeed > 10f)                      //滑らかに加減速出来る様加速度の値に応じて変数の挙動を変えている
            {                                                                       //各加減速毎にParticleの変化も入れた
                m_moveSpeed -= 1f;
                rightMa.startSize = new ParticleSystem.MinMaxCurve(0.5f, 0.8f);     
                leftMa.startSize = new ParticleSystem.MinMaxCurve(0.5f, 0.8f);
            }
            else if (m_inputAcceleration == 0f)
            {
                if (m_moveSpeed < 30f)
                {
                    m_moveSpeed += 1f;
                }
                else if (m_moveSpeed >= 30f || m_moveSpeed <= 40f)
                {
                    m_moveSpeed = 35f;
                    rightMa.startSize = new ParticleSystem.MinMaxCurve(1.2f, 1.4f);
                    leftMa.startSize = new ParticleSystem.MinMaxCurve(1.2f, 1.4f);
                }
                else if (m_moveSpeed > 40f)
                {
                    m_moveSpeed -= 1f;
                }
            }
            else if (m_inputAcceleration > 0f && m_moveSpeed < 60f)
            {
                m_moveSpeed += 1f;
                rightMa.startSize = new ParticleSystem.MinMaxCurve(2f, 2.5f);
                leftMa.startSize = new ParticleSystem.MinMaxCurve(2f, 2.5f);
            }
        }
    }
}



