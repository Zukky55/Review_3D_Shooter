using UnityEngine;
using System.Collections;

public class BoltController : MonoBehaviour
{
    /// <summary>Speed of object</summary>
    [SerializeField] float m_speed;
    /// <summary>explosion of enemy</summary>
    [SerializeField] GameObject m_eEnemy;
    /// <summary>explosion of asteroid</summary>
    [SerializeField] GameObject m_eAsteroid;
    private Rigidbody m_rb;
    private Vector3 m_diff;
    private ObjectController m_oc;
    /// <summary>Attack power of Bolt</summary>
    [SerializeField] int m_boltDamage = 3;
    private GameObject m_motherShip;

    /// <summary>Processing when a bolt is hit</summary>
    /// <param name="other">Bumped object</param>
    private void OnParticleCollision(GameObject other)
    {
        m_oc = other.GetComponent<ObjectController>();
        
        if (m_oc.m_myStatus.hitPoint > m_boltDamage && m_oc.m_type != Type.MotherShip)                    //If HP is greater than the attack power will give damage
        {
            m_oc.m_myStatus.hitPoint -= m_boltDamage;
        }
        else
        {
            switch (m_oc.m_type)
            {
                case Type.Planet:
                    Destroy(Instantiate(m_eAsteroid, other.transform.position, other.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(other.transform.gameObject);
                    break;
                case Type.Asteroid:
                    Destroy(Instantiate(m_eAsteroid, other.transform.position, other.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(other.transform.gameObject);
                    break;
                case Type.WeakEnemy:
                    Destroy(Instantiate(m_eAsteroid, other.transform.position, other.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(other.transform.gameObject);
                    break;
                case Type.NormalEnemy:
                    Destroy(Instantiate(m_eAsteroid, other.transform.position, other.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(other.transform.gameObject);
                    break;
                case Type.StrongEnemy:
                    Destroy(Instantiate(m_eAsteroid, other.transform.position, other.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(other.transform.gameObject);
                    break;
                case Type.MotherShip:
                    break;
                case Type.Other:
                    break;
                default:
                    break;
            }
        }
        Destroy(gameObject);
    }

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_motherShip = GameObject.Find("MotherShip");
        m_rb.velocity = transform.forward * m_speed;                                // Shot injection speed
    }

    private void Update()
    {
        m_diff = m_motherShip.transform.position - transform.position;
        if(m_diff.magnitude > 2000f)
        {
            Destroy(gameObject);
        }
    }

}