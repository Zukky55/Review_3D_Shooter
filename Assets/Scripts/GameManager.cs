using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Text m_text;
    private GameObject m_player;
    private PlayerController m_pc; 

    private void Initialize()
    {
        m_player = GameObject.Find("Player");                         //Get Player's component
        m_pc = m_player.GetComponent<PlayerController>();
        m_text = GameObject.Find("Text").GetComponent<Text>();        //Display message in text
        m_text.text = "PRESS SPACE \n TO START";
    }


    void Start()
    {
        Initialize();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_text.text = "";
        }
    }
}
