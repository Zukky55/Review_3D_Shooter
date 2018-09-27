using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipController : MonoBehaviour
{
    private Rigidbody m_rb;
    [SerializeField] private float m_speed;
    /// <summary>Laserのスポーンポイント</summary>
    [SerializeField] public Transform m_trackingLaserSpawn_1;
    [SerializeField] public Transform m_trackingLaserSpawn_2;
    /// <summary>レーザーのファイヤーレート</summary>
    [SerializeField] float m_laserFireRate;
    /// <summary>Laser</summary>
    [SerializeField] public GameObject m_laser;

    //IEnumerator TrackingLaser()
    //{
    //    if (m_isLaserRunning)
    //    {
    //        yield break;
    //    }
    //    m_isLaserRunning = true;

    //    Instantiate(m_laser, m_trackingLaserSpawn_1.position, m_trackingLaserSpawn_1.rotation);
    //    Instantiate(m_laser, m_trackingLaserSpawn_2.position, m_trackingLaserSpawn_2.rotation);
    //    yield return new WaitForSeconds(m_laserFireRate);
    //    m_isLaserRunning = false;
    //}

    private void Init()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Move()
    {
        m_rb.transform.position = transform.position + transform.forward * m_speed;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Move();
        //if (GameManager.m_startFlag)
        //{
        //    StartCoroutine(TrackingLaser());
        //}
    }
}
