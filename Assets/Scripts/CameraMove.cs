using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] GameObject m_player;
    GameObject m_cameraPos;
    private Rigidbody m_rb;
    private Quaternion m_rot;
    private Quaternion m_r;
    [SerializeField] float x = 40f;
    private Vector3 m_diff;
    [SerializeField] float m_ac = 10f;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_cameraPos = GameObject.Find("CameraPos");

    }

    private void FixedUpdate()
    {
        m_diff = m_player.transform.position - transform.position;
        m_rb.velocity = m_diff * m_ac;
        //transform.position = m_cameraPos.transform.position;
        //m_rot = Quaternion.LookRotation(m_diff, m_player.transform.up);
        m_r = m_rot * Quaternion.Inverse(transform.rotation);
        if (m_r.w < 0f)
        {
            m_r.x = -m_r.x;
            m_r.y = -m_r.y;
            m_r.z = -m_r.z;
            m_r.w = -m_r.w;
        }
        m_rb.AddTorque(new Vector3(m_r.x, m_r.y, m_r.z) * x, ForceMode.VelocityChange);
        transform.rotation = m_rot;
    }

}
