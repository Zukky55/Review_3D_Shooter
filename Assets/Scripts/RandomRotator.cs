using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class RandomRotator : MonoBehaviour
{
    public float tumble;
    Rigidbody m_rb;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.angularVelocity = Random.insideUnitSphere * tumble;
    }
}