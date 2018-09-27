using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingLaser : MonoBehaviour
{
    /// <summary>explosion of enemy</summary>
    [SerializeField] GameObject m_eEnemy;
    /// <summary>explosion of asteroid</summary>
    [SerializeField] GameObject m_eAsteroid;
    /// <summary>ObjectController.cs</summary>
    private ObjectController m_oc;
    /// <summary>Attack power of Bolt</summary>
    [SerializeField] int m_missileAtk = 5;
    /// <summary>Targetとの差分</summary>
    private Vector3 m_fromTargetDiff;
    /// <summary>Targets</summary>
    private GameObject[] m_asteroids;
    private GameObject[] m_enemies;
    private List<GameObject> m_targets = new List<GameObject>();
    private GameObject m_target;
    private GameObject[] m_targetObjects;
    private int m_index;
    /// <summary></summary>
    [SerializeField] private float m_detectDistance = 1000f;
    /// <summary>MotherShip</summary>
    private GameObject m_motherShip;
    /// <summary>distane from MotherShip</summary>
    private Vector3 m_diffFromMotherShip;
    private Rigidbody m_rb;
    /// <summary>ある速度で動く物体</summary>
    private Vector3 m_velocity;
    /// <summary>座標</summary>
    private Vector3 m_position;
    /// <summary>着弾迄の時間</summary>
    [SerializeField] private float m_period = 5f;
    /// <summary>加速度</summary>
    private Vector3 m_acceleration;
    /// <summary>差分</summary>
    private Vector3 m_diff;
    /// <summary>レーザーの軌道上に船がいる場合に軌道を逸らす為のフラグ(境界線)</summary>
    [SerializeField] private float m_boundary = 300f;







    /// <summary>Processing when a bolt is hit</summary>
    /// <param name="obj">Bumped object</param>
    private void OnParticleCollision(GameObject obj)
    {
        if (m_oc = obj.GetComponent<ObjectController>())
        {
            switch (m_oc.m_type)
            {
                case Type.Planet:
                    if (m_oc.m_myStatus.hitPoint > m_missileAtk) //HPが攻撃力より高い場合ダメージを与える。HP < Damageの場合破壊
                    {
                        m_oc.m_myStatus.hitPoint -= m_missileAtk;
                        m_oc.GetAnimator().SetTrigger("Damage");
                        Debug.Log(obj.name + "に当たったよ");
                        Destroy(gameObject);

                    }
                    else
                    {
                        Destroy(Instantiate(m_eAsteroid, obj.transform.position, obj.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                        GameManager.AddScore(m_oc.m_myStatus.point);
                        MainManager.ShowScore();
                        Destroy(obj.transform.gameObject);
                        Destroy(gameObject);
                    }
                    break;
                case Type.Asteroid:
                    if (m_oc.m_myStatus.hitPoint > m_missileAtk)
                    {
                        m_oc.m_myStatus.hitPoint -= m_missileAtk;
                        m_oc.GetAnimator().SetTrigger("Damage");
                        Debug.Log(obj.name + "に当たったよ");
                        Destroy(gameObject);

                    }
                    else
                    {
                        Destroy(Instantiate(m_eAsteroid, obj.transform.position, obj.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                        GameManager.AddScore(m_oc.m_myStatus.point);
                        MainManager.ShowScore();
                        Destroy(obj.transform.gameObject);
                        Destroy(gameObject);
                    }
                    break;
                case Type.WeakEnemy:
                case Type.NormalEnemy:
                case Type.StrongEnemy:
                    if (m_oc.m_myStatus.hitPoint > m_missileAtk)
                    {
                        var enemyCon = obj.GetComponent<EnemyController>(); //ダメージを受けた時色を変えるアニメーション
                        enemyCon.GetAnimator().SetTrigger("Damage");
                        m_oc.m_myStatus.hitPoint -= m_missileAtk;
                        Debug.Log(obj.name + "に当たったよ");
                        Destroy(gameObject);

                    }
                    else
                    {
                        Destroy(Instantiate(m_eEnemy, obj.transform.position, obj.transform.rotation), m_eEnemy.GetComponent<ParticleSystem>().main.duration);
                        GameManager.AddScore(m_oc.m_myStatus.point);
                        Destroy(obj.transform.gameObject);
                        MainManager.ShowScore();
                        Destroy(gameObject);
                    }
                    break;
                case Type.MotherShip:
                case Type.Other:
                case Type.Player:
                default:
                    break;
            }
        }
    }


    /// <summary>一定範囲内の敵オブジェクトの中からランダムで1つのGameObjectの情報を返す</summary>
    /// <returns>一定範囲内のオブジェクトからランダムに1つ返す</returns>
    private void GetTarget()
    {

        m_asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        m_enemies = GameObject.FindGameObjectsWithTag("Enemy");


        foreach (var obj in m_asteroids)
        {
            var diff = obj.transform.position - transform.position;
            if (diff.magnitude < m_detectDistance)
            {
                m_targets.Add(obj);
            }
        }
        foreach (var obj in m_enemies)
        {
            var diff = obj.transform.position - transform.position;
            if (diff.magnitude < m_detectDistance)
            {
                m_targets.Add(obj);
            }
        }

        if (m_targets.Count > 0)
        {
            m_target = m_targets[0]; //どれでもいいから最初に評価する為のobjectを代入する
            foreach (var obj in m_targets)
            {
                var targetDiff = m_target.transform.position - transform.position;
                var objDiff = obj.transform.position - transform.position;
                if (targetDiff.magnitude > objDiff.magnitude) //Listに入れたobjectの中でmagnitudeが一番
                {
                    m_target = obj; //レーザーオブジェクトから一番距離が近いobjectを代入する
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LaserShot()
    {
        if (m_target != null && GameManager.m_startFlag)
        {
            m_acceleration = transform.position;

            m_diff = m_target.transform.position - transform.position;
            m_acceleration += (m_diff - m_velocity * m_period) * 2f / (m_period * m_period);

            m_period -= Time.deltaTime;
            m_velocity += m_acceleration * Time.deltaTime;
            transform.position += m_velocity * Time.deltaTime; ;
        }
        else if (m_target == null)
        {
            //Debug.Log("m_target ぬるだお！！！！！！！");
        }

        if (m_motherShip != null) // MotherShipから一定距離離れたら自己破壊
        {
            m_diffFromMotherShip = (m_motherShip.transform.position + (m_motherShip.transform.forward * GameManager.m_multiply)) - transform.position;
        }
        if (m_diffFromMotherShip.magnitude > 3000f)
        {
            Destroy(gameObject);
        }

        if (m_velocity == Vector3.zero)
        {
            Destroy(gameObject);
        }
    }

    private void AvoidBoundaries()
    {
        var diff = m_motherShip.transform.position - transform.position;
        if(diff.magnitude < m_boundary)
        {
            m_rb.velocity = -diff;
        }
    }

    private void Init()
    {
        m_rb = GetComponent<Rigidbody>();
        m_motherShip = GameObject.Find("MotherShip");
        m_period = m_period * Random.Range(0.5f, 1.5f); //乱数を仕込んで同時に射出しても着弾のタイミングをずらす様にする
    }

void Start()
    {
        Init();
        GetTarget();
        if(m_target == null)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        LaserShot();
    }
}