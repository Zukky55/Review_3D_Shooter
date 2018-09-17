using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    /// <summary>Time of fade out</summary>
    [SerializeField] float m_fadeTime = 3f;
    public void OnClick()
    {
        FadeManager.FadeOut(1, m_fadeTime);
    }
}
