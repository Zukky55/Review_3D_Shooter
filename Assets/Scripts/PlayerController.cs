using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>マップの境界</summary>
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    /// <summary>Player's rigidbody</summary>
    Rigidbody m_rb;
    /// <summary>境界</summary>
    [SerializeField] Boundary m_boundary;
    /// <summary>弾</summary>
    [SerializeField]  public GameObject m_shot;
    /// <summary>弾のスポーンポイント</summary>
    [SerializeField] public Transform m_shotSpawn;
    /// <summary>弾のファイヤーレート</summary>
    [SerializeField] float m_fireRate;
    /// <summary>弾の音</summary>
    AudioSource m_bs;
    /// <summary>Player's speed ()</summary>
    [SerializeField] private float m_moveSpeed;
    public float m_setMoveSpeed
    {
        get { return m_moveSpeed; }
        set { m_moveSpeed = value; }
    }
    /// <summary>Player's Turning speed</summary>
    [SerializeField] float m_turningSpeed;
    /// <summary>フレーム終了直前のposition</summary>
    private Vector3 m_offsetPosition;
    /// <summary>Input axis value</summary>
    private float m_horizontal, m_vertical;
    private bool m_isRunning = false;




    /// <summary>機体を自動的に前進させる</summary>
    void Move()
    {
        m_rb.velocity = transform.forward * m_moveSpeed; 
    }

    void Turn ()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        var tilt = Quaternion.AngleAxis(-x * m_turningSpeed * Time.deltaTime, transform.forward);
        //Quaternion yAxisRot = Quaternion.AngleAxis(x * m_turnAcceleration * Time.deltaTime, Vector3.up);
        Quaternion xAxisRot = Quaternion.AngleAxis(y * m_turningSpeed * Time.deltaTime, transform.right);
        // 元の回転値と合成して上書き
        transform.rotation =tilt * xAxisRot * transform.rotation;        

    }

    IEnumerator WeaponShot()
    {
        if (m_isRunning)
        {
            yield break;
        }
        m_isRunning = true;


        Instantiate(m_shot, m_shotSpawn.position, m_shotSpawn.rotation);
        m_bs = GetComponent<AudioSource>();
        m_bs.Play();
        yield return new WaitForSeconds(m_fireRate);
        m_isRunning = false;
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(WeaponShot());
        }
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }
}



