using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDeathRay : MonoBehaviour
{
    /// <summary>All Target</summary>
    public GameObject m_target;
    private Rigidbody m_rb;
    /// <summary>ある速度で動く物体</summary>
    private Vector3 m_velocity;
    /// <summary>座標</summary>
    private Vector3 m_position;
    /// <summary>着弾迄の時間</summary>
    [SerializeField] private float m_period = 2f;
    /// <summary>加速度</summary>
    private Vector3 m_acceleration;
    /// <summary>差分</summary>
    private Vector3 m_diff;
    /// <summary>レーザーの軌道上に船がいる場合に軌道を逸らす為のフラグ(境界線)</summary>
    [SerializeField] private float m_boundary = 300f;
    /// <summary>MotherShip</summary>
    private GameObject m_motherShip;
    /// <summary>ObjectController.cs</summary>
    private ObjectController m_oc;
    /// <summary>explosion of enemy</summary>
    [SerializeField] GameObject m_eEnemy;
    /// <summary>explosion of asteroid</summary>
    [SerializeField] GameObject m_eAsteroid;
    /// <summary>MotherShipからの距離</summary>
    private Vector3 m_fromMotherShip;

    ///// <summary>Processing when a bolt is hit</summary>
    ///// <param name="obj">Bumped object</param>
    //private void OnParticleCollision(GameObject obj)
    //{
    //    if (m_oc = obj.GetComponent<ObjectController>())
    //    {
    //        switch (m_oc.m_type)
    //        {
    //            case Type.Asteroid:
    //                Destroy(Instantiate(m_eAsteroid, obj.transform.position, obj.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
    //                Destroy(obj.transform.gameObject);
    //                Destroy(gameObject);
    //                break;
    //            case Type.WeakEnemy:
    //            case Type.NormalEnemy:
    //            case Type.StrongEnemy:
    //                Destroy(Instantiate(m_eEnemy, obj.transform.position, obj.transform.rotation), m_eEnemy.GetComponent<ParticleSystem>().main.duration);
    //                Destroy(obj.transform.gameObject);
    //                Destroy(gameObject);
    //                break;
    //            case Type.Planet:
    //            case Type.MotherShip:
    //            case Type.Other:
    //            case Type.Player:
    //            default:
    //                break;
    //        }
    //    }
    //}

    /// <summary>発射</summary>
    private void Fire()
    {
        if (m_target != null)
        {
            m_diff = m_target.transform.position - transform.position;
            m_acceleration += (m_diff - m_velocity * m_period) * 2f / (m_period * m_period);
            m_period -= Time.deltaTime;
            m_velocity += m_acceleration * Time.deltaTime;
            transform.position += m_velocity * Time.deltaTime;
            m_acceleration = transform.position;
        }
        else
        {
            m_rb.velocity = transform.forward * -1000f;
        }

        if (m_period < 0f && m_target != null)
        {
            if (m_target.tag == "Asteroid")
            {
                Destroy(Instantiate(m_eAsteroid, m_target.transform.position, m_target.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                Destroy(m_target);
            }
            if (m_target.tag == "Enemy")
            {
                Destroy(Instantiate(m_eEnemy, m_target.transform.position, m_target.transform.rotation), m_eEnemy.GetComponent<ParticleSystem>().main.duration);
                Destroy(m_target);
            }


        }
    }

    private void AvoidBoundaries()
    {
        m_fromMotherShip = m_motherShip.transform.position - transform.position;
        if (m_fromMotherShip.magnitude < m_boundary)
        {
            m_rb.velocity = -m_fromMotherShip;
        }
    }

    private void Init()
    {
        m_rb = GetComponent<Rigidbody>();
        m_motherShip = GameObject.Find("MotherShip");
        m_period = m_period * Random.Range(0.5f, 2f); //乱数を仕込んで同時に射出しても着弾のタイミングをずらす様にする
    }

    void Start()
    {
        Init();
        AvoidBoundaries();
    }

    private void Update()
    {
        Fire();

        if (m_motherShip != null) // MotherShipから一定距離離れたら自己破壊
        {
            m_diff = (m_motherShip.transform.position + (m_motherShip.transform.forward * GameManager.m_multiply)) - transform.position;
        }
        if (m_diff.magnitude > 3000f)
        {
            Destroy(gameObject);
        }
    }
}
