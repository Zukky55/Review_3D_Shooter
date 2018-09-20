using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private float m_speed;

    private void Update()
    {
        var pos = GameObject.Find("MotherShip").transform.position;
        var diff = pos + transform.forward * m_speed  - transform.position;
        Debug.Log("diff = " + diff.magnitude);
    }
}
