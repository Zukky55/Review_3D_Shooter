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
    [SerializeField] int m_boltAtk = 3;
    private GameObject m_motherShip;

    /// <summary>Processing when a bolt is hit</summary>
    /// <param name="obj">Bumped object</param>
    private void OnParticleCollision(GameObject obj)
    {
        if(m_oc = obj.GetComponent<ObjectController>())
        {
            switch (m_oc.m_type)
            {
                case Type.Planet:
                    if (m_oc.m_myStatus.hitPoint > m_boltAtk) //HPが攻撃力より高い場合ダメージを与える。HP < Damageの場合破壊
                    {
                        m_oc.m_myStatus.hitPoint -= m_boltAtk;
                        m_oc.GetAnimator().SetTrigger("Damage");
                        Debug.Log("Planet's HP is: " + m_oc.m_myStatus.hitPoint);
                    }
                    else
                    {
                        Destroy(Instantiate(m_eAsteroid, obj.transform.position, obj.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                        GameManager.AddScore(m_oc.m_myStatus.point);
                        MainManager.ShowScore();
                        Destroy(obj.transform.gameObject);
                    }
                    break;
                case Type.Asteroid:
                    if (m_oc.m_myStatus.hitPoint > m_boltAtk)
                    {
                        m_oc.m_myStatus.hitPoint -= m_boltAtk;
                        m_oc.GetAnimator().SetTrigger("Damage");
                        Debug.Log("Asteroid's HP is: " + m_oc.m_myStatus.hitPoint);

                    }
                    else
                    {
                        Destroy(Instantiate(m_eAsteroid, obj.transform.position, obj.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);
                        GameManager.AddScore(m_oc.m_myStatus.point);
                        MainManager.ShowScore();
                        Destroy(obj.transform.gameObject);
                    }
                    break;
                case Type.WeakEnemy:
                case Type.NormalEnemy:
                case Type.StrongEnemy:
                    if (m_oc.m_myStatus.hitPoint > m_boltAtk)
                    {
                        var enemyCon = obj.GetComponent<EnemyController>(); //ダメージを受けた時色を変えるアニメーション
                        enemyCon.GetAnimator().SetTrigger("Damage");
                        m_oc.m_myStatus.hitPoint -= m_boltAtk;
                        Debug.Log("Enemy's HP is: " + m_oc.m_myStatus.hitPoint);
                    }
                    else
                    {
                        Destroy(Instantiate(m_eEnemy, obj.transform.position, obj.transform.rotation), m_eEnemy.GetComponent<ParticleSystem>().main.duration);
                        GameManager.AddScore(m_oc.m_myStatus.point);
                        Destroy(obj.transform.gameObject);
                        MainManager.ShowScore();
                    }
                    break;
                case Type.MotherShip:
                case Type.Other:
                case Type.Player:
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
        if (m_diff.magnitude > 1500f)
        {
            Destroy(gameObject);
        }
    }

}