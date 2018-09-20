using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Type of object</summary>
public enum Type
{
    Asteroid,
    WeakEnemy,
    NormalEnemy,
    StrongEnemy,
    Planet,
    MotherShip,
    Other
}

/// <summary>Status of the object</summary>
public struct MyStatus
{
    /// <summary>Object moving speed</summary>
    public float speed;
    /// <summary>Points when destroying that object</summary>
    public int point;
    /// <summary>Hit point of the object</summary>
    public int hitPoint;
    /// <summary>Object rotation speed</summary>
    public float tilit;

}

//Request for forced attachment of attached object
[RequireComponent(typeof(Rigidbody))]

public class ObjectController : MonoBehaviour
{

    public MyStatus m_myStatus;
    [SerializeField] public Type m_type;
    private Rigidbody m_rb;
    /// <summary>Random value of vector</summary>
    private Vector3 m_vec;

    private void Initialize()
    {
        m_vec = Random.insideUnitSphere;                        //球体の-1~1の範囲で乱数を代入する

        switch (m_type)                                         //type毎にstatusを設定代入する
        {
            case Type.Planet:
                m_myStatus.point = 100;
                m_myStatus.hitPoint = 1;
                m_myStatus.tilit = 1f;
                break;
            case Type.Asteroid:
                m_myStatus.point = 300;
                m_myStatus.hitPoint = 1;
                m_myStatus.tilit = 1f;
                break;
            case Type.WeakEnemy:
                m_myStatus.point = 500;
                m_myStatus.hitPoint = 5;
                m_myStatus.tilit = 0f;
                break;
            case Type.NormalEnemy:
                m_myStatus.point = 700;
                m_myStatus.hitPoint = 5;
                m_myStatus.tilit = 0f;
                break;
            case Type.StrongEnemy:
                m_myStatus.point = 1000;
                m_myStatus.hitPoint = 5;
                m_myStatus.tilit = 0f;
                break;
            case Type.MotherShip:
                m_myStatus.point = 0;
                m_myStatus.hitPoint = 5000;
                m_myStatus.tilit = 0f;
                break;
            case Type.Other:
                m_myStatus.point = 1000;
                m_myStatus.hitPoint = 5;
                m_myStatus.tilit = 0.5f;
                break;

            default:
                break;
        }
    }

    private void Start()
    {
        Initialize();
    }

    void Update()
    {

        switch (m_type)
        {
            case Type.Planet:
                transform.Rotate(m_vec * m_myStatus.tilit, Space.World);
                break;
            case Type.Asteroid:
                transform.Rotate(m_vec * m_myStatus.tilit, Space.World);
                break;
            case Type.WeakEnemy:
                break;
            case Type.NormalEnemy:
                break;
            case Type.StrongEnemy:
                break;
            case Type.MotherShip:
                break;
            case Type.Other:
                transform.Rotate(m_vec * m_myStatus.tilit, Space.World);
                break;
            default:
                break;
        }
    }
}
