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
    Other,
    Player
}
/// <summary>Status of the object</summary>
[System.Serializable]
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
    /// <summary>Attack power of object</summary>
    public int attack;
    /// <summary>Rate to shoot bullets</summary>
    public float fireRate;
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
    private Animator m_animator;

    public Animator GetAnimator()
    {
        return m_animator;
    }

    private void Init()
    {
        m_vec = Random.insideUnitSphere;                        //球体の-1~1の範囲で乱数を代入する
        switch (m_type)                                         //type毎にstatusを設定代入する
        {
            case Type.Planet:
                m_myStatus.point = 100;
                m_myStatus.hitPoint = 1;
                m_myStatus.tilit = 1f;
                m_myStatus.attack = 0;
                m_animator = GetComponent<Animator>();
                break;
            case Type.Asteroid:
                m_myStatus.point = 300;
                m_myStatus.hitPoint = 1;
                m_myStatus.tilit = 1f;
                m_myStatus.attack = 10;
                m_myStatus.speed = 10f;
                m_animator = GetComponent<Animator>();
                break;
            case Type.WeakEnemy:
                m_myStatus.point = 500;
                m_myStatus.hitPoint = 1;
                m_myStatus.tilit = 0f;
                m_myStatus.attack = 1;
                m_myStatus.speed = 75f;
                m_myStatus.fireRate = 1f;
                m_animator = GetComponent<Animator>();
                break;
            case Type.NormalEnemy:
                m_myStatus.point = 700;
                m_myStatus.hitPoint = 15;
                m_myStatus.tilit = 0f;
                m_myStatus.attack = 3;
                m_myStatus.speed = 35f;
                m_myStatus.fireRate = 2f;
                m_animator = GetComponent<Animator>();
                break;
            case Type.StrongEnemy:
                m_myStatus.point = 1000;
                m_myStatus.hitPoint = 30;
                m_myStatus.tilit = 0f;
                m_myStatus.attack = 5;
                m_myStatus.speed = 35f;
                m_myStatus.fireRate = 1f;
                m_animator = GetComponent<Animator>();
                break;
            case Type.MotherShip:
                m_myStatus.point = 0;
                m_myStatus.hitPoint = 3000;
                m_myStatus.tilit = 0f;
                m_myStatus.attack = 0;
                m_animator = GameObject.Find("Ship").GetComponent<Animator>();
                break;
            case Type.Other:
                m_myStatus.point = 1000;
                m_myStatus.hitPoint = 1;
                m_myStatus.tilit = 0.1f;
                m_myStatus.attack = 0;
                break;
            case Type.Player:
                m_myStatus.hitPoint = 300;
                break;
            default:
                break;
        }
    }
    private void Start()
    {
        Init();
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