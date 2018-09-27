using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    /// <summary>Player's rigidbody</summary>
    Rigidbody m_rb;
    /// <summary>弾</summary>
    [SerializeField]  public GameObject m_shot;
    /// <summary>Laser</summary>
    [SerializeField]  public GameObject m_laser;
    /// <summary>弾のスポーンポイント</summary>
    [SerializeField] public Transform m_shotSpawn;
    /// <summary>弾のファイヤーレート</summary>
    [SerializeField] float m_weaponFireRate;
    /// <summary>レーザーのファイヤーレート</summary>
    [SerializeField] float m_laserFireRate;
    /// <summary>AudioSource</summary>
    private AudioSource m_audioS;
    /// <summary>弾の音</summary>
    [SerializeField] AudioClip m_weaponSE;
    /// <summary>レーザーの音</summary>
    [SerializeField] AudioClip m_laserSE;
    /// <summary>Player's Turning speed</summary>
    [SerializeField] float m_turningSpeed;
    /// <summary>重複呼び出し防止フラグ</summary>
    private bool m_isWeaponRunning = false;
    ParticleSystem m_rightAfterBurner;
    ParticleSystem m_leftAfterBurner;
    /// <summary>現在地から初期位置を引いた差分</summary>
    private Vector3 m_diff;
    /// <summary>ステージ制限範囲を超えた時に旋回させる回転速度</summary>
    [SerializeField] float m_rotateTime;
    /// <summary>Input acceleration</summary>
    private float m_inputAcceleration;
    /// <summary>Mother ship</summary>
    private GameObject m_motherShip;
    /// <summary>Set Player's speed</summary>
    [SerializeField] private float m_setMovingSpeed;
    /// <summary>Player's speed</summary>    
    private float m_movingSpeed;
    [SerializeField] private float m_spawnAngle = 5f;
    private bool m_particleFlag = true;
    /// <summary>explosion of enemy</summary>
    [SerializeField] GameObject m_eEnemy;
    /// <summary>explosion of asteroid</summary>
    [SerializeField] GameObject m_eAsteroid;



    /// <summary>機体を前進させる</summary>
    void Move()
    {
        m_rb.velocity = transform.forward * m_movingSpeed;                                                                                                  //常に機体の前方方向へ前進
        if(m_motherShip != null)
        {
            m_diff = (m_motherShip.transform.position + (m_motherShip.transform.forward * GameManager.m_multiply)) - transform.position;                                  //Vector acquisition to initial position
        }
        if (m_diff.magnitude > 1250f)                                                                                                                     //指定範囲を超えたら強制的に方向転換
        {
            var targetRotation = Quaternion.LookRotation(m_diff, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / m_rotateTime);
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


    private void Accelerator()
    { 
        var rightMa = m_rightAfterBurner.main;                                  //ParticleSystem.main of RightAfterBurner
        var leftMa = m_leftAfterBurner.main;                                    //ParticleSystem.main of LeftAfterBurner
        m_inputAcceleration = Input.GetAxis("Acceleration");                                        //Inputを自分でカスタマイズしてSpaceで減速,Shiftで加速にしている

        if (m_particleFlag)
        {
            m_rightAfterBurner.Play();                                          //Play particle system
            m_leftAfterBurner.Play();
            m_particleFlag = false;
        }


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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "MotherShip")
        {
            var diff = transform.position - collision.transform.position;
            var targetRotation = Quaternion.LookRotation(diff, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotateTime);
        }
    }


    IEnumerator WeaponShot()
    {
        if (m_isWeaponRunning)
        {
            yield break;
        }
        m_isWeaponRunning = true;

        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation);
        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation * Quaternion.Euler(transform.forward + (Vector3.up * m_spawnAngle)));
        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation * Quaternion.Euler(transform.forward + (Vector3.up * -m_spawnAngle)));
        m_audioS.PlayOneShot(m_weaponSE); //Play SE
        yield return new WaitForSeconds(m_weaponFireRate);
        m_isWeaponRunning = false;
    }

    //IEnumerator TrackingLaser()
    //{
    //    if (m_isLaserRunning)
    //    {
    //        yield break;
    //    }
    //    m_isLaserRunning = true;

    //    m_audioS.PlayOneShot(m_laserSE); //Play SE
    //    Instantiate(m_laser, m_trackingLaserSpawn.position, m_trackingLaserSpawn.rotation);
    //    yield return new WaitForSeconds(m_laserFireRate);
    //    m_isLaserRunning = false;
    //}

    private void Init()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rightAfterBurner = GameObject.Find("RightAfterburner").GetComponent<ParticleSystem>();
        m_leftAfterBurner = GameObject.Find("LeftAfterburner").GetComponent<ParticleSystem>();
        m_motherShip = GameObject.Find("MotherShip");
        m_audioS = GetComponent<AudioSource>();

        m_rb.freezeRotation = true; //物理演算による回転の影響を無視させる
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (GameManager.m_startFlag)
        {
            StartCoroutine(WeaponShot());
            //StartCoroutine(TrackingLaser());
        }
    }

    private void FixedUpdate()
    {

        Move();                                     //Method to move
        Turn();                                     //Method to turning player
        Accelerator();                             //Method to accelerator

    }
}



