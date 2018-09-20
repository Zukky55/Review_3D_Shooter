using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipController : MonoBehaviour
{
    private Rigidbody m_rb;
    [SerializeField] private float m_speed;




    private void Initialize()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Move()
    {
        m_rb.transform.position = transform.position + transform.forward * m_speed;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Move();
    }
}
