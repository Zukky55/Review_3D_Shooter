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
    Rigidbody m_rb;
    GameObject m_player;
    PlayerController m_pc;
    ParticleSystem m_parA;
    ParticleSystem m_parE;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_player = GameObject.Find("Player");
        m_pc = m_player.GetComponent<PlayerController>();
        // playerの動きに弾の速度を合わせる為プレイヤーが移動している時はそのスカラー量も掛ける
        m_rb.velocity = transform.forward * m_speed;
        m_parA = m_eAsteroid.GetComponent<ParticleSystem>();
        m_parE = m_eEnemy.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Asteroid":
                Instantiate(m_eAsteroid, collision.transform.position, collision.transform.rotation);
                Destroy(collision.transform.gameObject);
                StartCoroutine(CheckIsAlive(collision.gameObject));
                break;
            case "Enemy":
                Instantiate(m_eEnemy, collision.transform.position, collision.transform.rotation);
                Destroy(collision.transform.gameObject);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boundary")
        {
            Destroy(this);
        }
    }
    IEnumerator CheckIsAlive(GameObject go)
    {
        var par = go.GetComponent<ParticleSystem>();
        yield return new WaitWhile(() => par.IsAlive());
    }
}