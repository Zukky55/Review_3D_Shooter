using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] float m_speed;
    private float m_rotation;

    void Update()
    {
        m_rotation += m_speed;
        if(360 <= m_rotation)
        {
            m_rotation -= 360;
        }
        var skybox = RenderSettings.skybox;
        skybox.SetFloat("_Rotation", m_rotation);
    }
}
