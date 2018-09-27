using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Rigidbody m_rb;
    /// <summary>ObjectController.cs</summary>
    private ObjectController m_oc;
    /// <summary>現在地から初期位置を引いた差分</summary>
    private Vector3 m_diff;
    /// <summary>Mother ship</summary>
    private GameObject m_motherShip;
    /// <summary>MotherShipのセンター</summary>
    private Vector3 m_MSPos;
    [SerializeField] GameObject m_explosion;



    private void Init()
    {
        m_rb = GetComponent<Rigidbody>();
        m_oc = GetComponent<ObjectController>();
        m_motherShip = GameObject.Find("MotherShip");
        if(m_motherShip != null)
        {
            m_MSPos = (m_motherShip.transform.position + (m_motherShip.transform.forward * GameManager.m_multiply));
        }
        m_diff = m_MSPos + Random.insideUnitSphere* 100f - transform.position;

        //m_rb.AddForce(m_diff*Time.deltaTime + Random.insideUnitSphere, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MotherShip" && GameManager.m_startFlag)
        {
            if (m_oc.m_myStatus.hitPoint > m_oc.m_myStatus.attack) //HPが攻撃力より高い場合ダメージを与える。HP < Damageの場合破壊
            {
                m_oc.m_myStatus.hitPoint -= m_oc.m_myStatus.attack;
                Destroy(Instantiate(m_explosion, transform.position, transform.rotation), m_explosion.GetComponent<ParticleSystem>().main.duration);
            }
            else
            {
                Destroy(Instantiate(m_explosion, collision.transform.position, transform.rotation), m_explosion.GetComponent<ParticleSystem>().main.duration); //爆破エフェクト
                Destroy(collision.transform.gameObject);
                AudioManager.ExplosionSound(); //爆発音を流す
                GameManager.m_gameOverFlag = true;
                GameManager.m_timerFlag = false;
            }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        m_rb.velocity = (m_diff.normalized * m_oc.m_myStatus.speed);
    }
}
