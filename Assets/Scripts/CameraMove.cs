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
    [SerializeField] private float m_setAc;
    private float m_ac = 0f;
    /// <summary>ステージ開始後カメラを滑らかにスピードアップさせるフラグ</summary>
    private bool m_flagAcceleratioin = true;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_cameraPos = GameObject.Find("CameraPos");
        m_ac = 0f;
    }

    private void FixedUpdate()
    {
            if (m_flagAcceleratioin)                    //加速度が一定に達する迄加速
            {
                if (m_ac > m_setAc)
                {
                    m_ac = m_setAc;
                    m_flagAcceleratioin = false;
                }
                m_ac += 0.1f;
            }

            m_diff = m_cameraPos.transform.position - transform.position;               //このm_diffはCameraPosition用のオブジェクトとカメラとの差異を計算してvelocityが向かう先はCameraPosにして減速しても変なカメラアングルにならない様に設定している
            m_rb.velocity = m_diff * m_ac;
            m_diff = m_player.transform.position - transform.position;                  //このm_diffはプレイヤーとの差異を取っており、視線は常にプレイヤーを追うようになっている
            m_rot = Quaternion.LookRotation(m_diff, m_player.transform.up);
            m_r = m_rot * Quaternion.Inverse(transform.rotation);                       //Multiply by the reciprocal of quaternion
            if (m_r.w < 0f)                                                             //Quaterion成分がマイナスの時はプラスに変えて近い方の回転に変換している
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
