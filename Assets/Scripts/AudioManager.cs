using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>BGM再生フラグ</summary>
    public static bool m_bgmFlag = false;
    /// <summary>BGM再生フラグ</summary>
    public static bool m_clearBgmFlag = false;
    /// <summary>bgm</summary>
    public static AudioSource m_audio;
    private static AudioClip m_explosion; 
    [SerializeField] private AudioClip m_setExplosion; 


    /// <summary>Play BGM</summary>
    public static void StartAudio()
    {
        m_audio.Play();
    }
    
    /// <summary>Stop BGM</summary>
    public static void StopBgm()
    {
        m_audio.Stop();
    }

    public static void ExplosionSound()
    {
        m_audio.PlayOneShot(m_explosion);
    }

    private void Init()
    {
        m_audio = GetComponent<AudioSource>();
        m_explosion = m_setExplosion;
    }

    private void Start()
    {
        Init();
    }
}
