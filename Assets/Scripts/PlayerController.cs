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
    /// <summary>Player's Turning speed</summary>
    [SerializeField] float m_turningSpeed;
    /// <summary>重複呼び出し防止フラグ</summary>
    private bool m_isRunning = false;
    ParticleSystem m_rightAfterBurner;
    ParticleSystem m_leftAfterBurner;
    /// <summary>ステージ開始時にパーティクルを再生するフラグ</summary>
    private bool m_particleFlag = true;
    /// <summary>現在地から初期位置を引いた差分</summary>
    private Vector3 m_diff;
    /// <summary>ステージ制限範囲を超えた時に旋回させる回転速度</summary>
    [SerializeField] float m_rotateTime;
    /// <summary>Input acceleration</summary>
    private float m_inputAcceleration;
    /// <summary>Mother ship</summary>
    private GameObject m_motherShip;
    [SerializeField] private float m_multiply = 450f;
    /// <summary>Set Player's speed</summary>
    [SerializeField] private float m_setMovingSpeed;
    /// <summary>Player's speed</summary>    
    private float m_movingSpeed;
    [SerializeField] private float m_spawnAngle = 5f;




    private void Initialize()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rightAfterBurner = GameObject.Find("RightAfterburner").GetComponent<ParticleSystem>();
        m_leftAfterBurner = GameObject.Find("LeftAfterburner").GetComponent<ParticleSystem>();
        m_motherShip = GameObject.Find("MotherShip");
    }

/// <summary>機体を前進させる</summary>
    void Move()
    {
        m_rb.velocity = transform.forward * m_movingSpeed;                                                                                                  //常に機体の前方方向へ前進

        m_diff = (m_motherShip.transform.position + (m_motherShip.transform.forward * m_multiply)) - transform.position;                                  //Vector acquisition to initial position
        if (m_diff.magnitude > 1500f)                                                                                                                     //指定範囲を超えたら強制的に方向転換
        {
            var targetRotation = Quaternion.LookRotation(m_diff, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotateTime);
        }
    }

    /// <summary>WASD keyのインプットに合わせて機体を旋回させる</summary>
    void Turn ()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Quaternion xAxisRot = Quaternion.AngleAxis(y * m_turningSpeed * Time.deltaTime, transform.right);
        Quaternion yAxisRot = Quaternion.AngleAxis(x * m_turningSpeed * Time.deltaTime, transform.up);
        transform.rotation = yAxisRot * xAxisRot * transform.rotation;                                                  // 元の回転値と合成して上書き

        //Quaternion tilt = Quaternion.AngleAxis(-1f * m_turningSpeed * Time.deltaTime, transform.forward);          //Rotate on the Z axis when X inputted

    }

    IEnumerator WeaponShot()
    {
        if (m_isRunning)
        {
            yield break;
        }
        m_isRunning = true;

        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation);
        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation * Quaternion.Euler(transform.forward + (Vector3.up * m_spawnAngle)));
        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation * Quaternion.Euler(transform.forward + (Vector3.up * -m_spawnAngle)));
        m_bs = GetComponent<AudioSource>();
        m_bs.Play();
        yield return new WaitForSeconds(m_fireRate);
        m_isRunning = false;
    }

    private void Accelerator()
    { 
        var rightMa = m_rightAfterBurner.main;                                  //ParticleSystem.main of RightAfterBurner
        var leftMa = m_leftAfterBurner.main;                                    //ParticleSystem.main of LeftAfterBurner
        
        if (m_particleFlag)
        {
            m_rightAfterBurner.Play();                                          //Play particle system
            m_leftAfterBurner.Play();
            m_particleFlag = false;
        }


        m_inputAcceleration = Input.GetAxis("Acceleration");                                        //Inputを自分でカスタマイズしてSpaceで減速,Shiftで加速にしている
        if (m_inputAcceleration < 0f && m_movingSpeed > m_setMovingSpeed - 25f)                     //滑らかに加減速出来る様加速度の値に応じて変数の挙動を変えている
        {                                                                                           //各加減速毎にParticleの変化も入れた
            m_movingSpeed -= 1f;
            rightMa.startSize = new ParticleSystem.MinMaxCurve(0.5f, 0.8f);
            leftMa.startSize = new ParticleSystem.MinMaxCurve(0.5f, 0.8f);
        }
        else if (m_inputAcceleration == 0f)
        {
            if (m_movingSpeed < m_setMovingSpeed - 5f)
            {
                m_movingSpeed += 1f;
            }
            else if (m_movingSpeed >= m_setMovingSpeed - 5f || m_movingSpeed <= m_setMovingSpeed + 5f)
            {
                m_movingSpeed = m_setMovingSpeed;
                rightMa.startSize = new ParticleSystem.MinMaxCurve(1.2f, 1.4f);
                leftMa.startSize = new ParticleSystem.MinMaxCurve(1.2f, 1.4f);
            }
            else if (m_movingSpeed > m_setMovingSpeed + 5f)
            {
                m_movingSpeed -= 1f;
            }
        }
        else if (m_inputAcceleration > 0f && m_movingSpeed < m_setMovingSpeed + 25f)
        {
            m_movingSpeed += 1f;
            rightMa.startSize = new ParticleSystem.MinMaxCurve(2f, 2.5f);
            leftMa.startSize = new ParticleSystem.MinMaxCurve(2f, 2.5f);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && GameManager.m_startFlag)
        {
            StartCoroutine(WeaponShot());
        }
    }

    private void FixedUpdate()
    {
        //If the game starts
        if (GameManager.m_startFlag)
        {
            Move();                                     //Method to move
            Turn();                                     //Method to turning player
            Accelerator();                             //Method to accelerator
        }
    }
}



