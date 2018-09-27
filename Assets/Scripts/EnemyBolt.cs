using UnityEngine;
using System.Collections;

public class EnemyBolt : MonoBehaviour
{
    /// <summary>Speed of object</summary>
    [SerializeField] float m_speed;
    /// <summary>explosion of MotherShip</summary>
    [SerializeField] private GameObject m_explosion;
    private Rigidbody m_rb;
    private Vector3 m_diff;
    private ObjectController m_oc;
    /// <summary>Attack power of Bolt</summary>
    [SerializeField] int m_boltDamage = 3;
    private GameObject m_motherShip;

    /// <summary>Processing when a bolt is hit</summary>
    /// <param name="obj">Bumped object</param>
    private void OnParticleCollision(GameObject obj)
    {
        m_oc = obj.GetComponent<ObjectController>();
        if(GameManager.m_startFlag)
        {
            switch (m_oc.m_type)
            {
                case Type.MotherShip:
                    if (m_oc.m_myStatus.hitPoint > m_boltDamage)
                    {
                        m_oc.GetAnimator().SetTrigger("Damage");
                        m_oc.m_myStatus.hitPoint -= m_boltDamage;
                        //Debug.Log("MotherShip's HP is: " + m_oc.m_myStatus.hitPoint);
                    }
                    else
                    {
                        Destroy(Instantiate(m_explosion, obj.transform.position, obj.transform.rotation), m_explosion.GetComponent<ParticleSystem>().main.duration); //爆破エフェクト
                        AudioManager.ExplosionSound(); //爆発音を流す
                        GameManager.m_gameOverFlag = true;
                        GameManager.m_timerFlag = false;
                        Destroy(obj.transform.gameObject);
                    }
                    break;

                case Type.Planet:
                case Type.Asteroid:
                case Type.WeakEnemy:
                case Type.NormalEnemy:
                case Type.StrongEnemy:
                case Type.Other:
                default:
                    break;
            }
        }
        Destroy(gameObject);
    }

    private void Init()
    {
        m_rb = GetComponent<Rigidbody>();
        m_motherShip = GameObject.Find("MotherShip");
    }

    void Start()
    {
        Init();
        m_rb.velocity = transform.forward * m_speed;                                // Shot injection speed
    }

    private void Update()
    {

        if (m_motherShip != null) // MotherShipから一定距離離れたら自己破壊
        {
            m_diff = (m_motherShip.transform.position + (m_motherShip.transform.forward * GameManager.m_multiply)) - transform.position;
        }
        if (m_diff.magnitude > 2000f)
        {
            Destroy(gameObject);
        }
    }

}