using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButton : MonoBehaviour
{
    public void OnClick()
    {
        FadeManager.FadeOut(0, 2f);
    }
}
