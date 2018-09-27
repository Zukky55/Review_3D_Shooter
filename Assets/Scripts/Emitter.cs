using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Waves
{
    Asteroids,SmallEnemies, MediumEnemies, BossEnemies
}

public class Emitter : MonoBehaviour
{
    /// <summary>Object to be spawned</summary>
    [SerializeField] private GameObject m_obj;
    /// <summary>List of objects to be spawned</summary>
    [SerializeField] private List<GameObject> m_objects = new List<GameObject>();
    /// <summary>Wave level</summary>
    [SerializeField] private Waves m_toggle;
    /// <summary>Range of random numbers for generating objects</summary>
    [SerializeField] private float m_rad;
    /// <summary>Center of MotherShip</summary>
    private Vector3 m_pos;
    [SerializeField] private GameObject m_targetObj;
    /// <summary>Frequency of generation</summary>
    [SerializeField] private float m_spawnRate;
    /// <summary>Mother ship</summary>
    private GameObject m_motherShip;
    /// <summary>MotherShipのセンター</summary>
    private Vector3 m_MSPos;
    /// <summary>差分</summary>
    private Vector3 m_diff;



    IEnumerator SpawnCoroutine()
    {
        switch (m_toggle)
        {
            case Waves.Asteroids:
                while(true)
                {
                    var pos = m_pos + Random.onUnitSphere * m_rad;
                    Instantiate(m_objects[Random.Range(0, m_objects.Count)], pos, Random.rotationUniform);
                    yield return new WaitForSeconds(m_spawnRate);
                    if(GameManager.m_clearFlag)
                    {
                        break;
                    }
                }
                break;
            case Waves.SmallEnemies:
                while (true)
                {
                    Instantiate(m_obj, m_pos + Random.onUnitSphere * m_rad, m_obj.transform.rotation);
                    yield return new WaitForSeconds(m_spawnRate);
                    if (GameManager.m_clearFlag)
                    {
                        break;
                    }
                }
                break;
            case Waves.MediumEnemies:
                yield return new WaitUntil(() => GameManager.m_startFlag);
                yield return new WaitForSeconds(10f);
                while (GameManager.m_startFlag)
                {
                    Instantiate(m_obj, m_pos + Random.onUnitSphere * m_rad, m_obj.transform.rotation);
                    yield return new WaitForSeconds(m_spawnRate);
                    if (GameManager.m_clearFlag)
                    {
                        break;
                    }

                }
                break;
            case Waves.BossEnemies:
                yield return new WaitUntil(() => GameManager.m_startFlag);
                yield return new WaitForSeconds(30f);
                while (GameManager.m_startFlag)
                {
                    Instantiate(m_obj, m_pos + Random.onUnitSphere * m_rad, m_obj.transform.rotation);
                    yield return new WaitForSeconds(m_spawnRate);
                    if (GameManager.m_clearFlag)
                    {
                        break;
                    }
                }
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }


    private void Init()
    {
        m_motherShip = GameObject.Find("MotherShip");
        m_pos = (m_motherShip.transform.position + (m_motherShip.transform.forward * GameManager.m_multiply)); //Center of MotherShip
    }

    private void Start()
    {
        Init();
        StartCoroutine(SpawnCoroutine());
    }
}
