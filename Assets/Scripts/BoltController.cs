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
    private void OnParticleCollision(GameObject other)
    {
        switch (other.gameObject.tag)
        {
            case "Asteroid":
                Instantiate(m_eAsteroid, other.transform.position, other.transform.rotation);
                Destroy(other.transform.gameObject);
                StartCoroutine(CheckIsAlive(m_eAsteroid));
                break;
            case "Enemy":
                Instantiate(m_eAsteroid, other.transform.position, other.transform.rotation);
                Destroy(other.transform.gameObject);
                StartCoroutine(CheckIsAlive(m_eAsteroid));
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    IEnumerator CheckIsAlive(GameObject go)
    {
        Debug.Log(go.gameObject.name);
        var par = go.GetComponent<ParticleSystem>();
        yield return new WaitWhile(() => !par.IsAlive());

        Debug.Log(par.IsAlive());
        Debug.Log(go.name);
        yield break;
    }
}