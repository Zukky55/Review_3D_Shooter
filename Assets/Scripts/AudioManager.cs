using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource[] m_audioSources;

    private void Start()
    {
        m_audioSources = GetComponents<AudioSource>();
    }
}


