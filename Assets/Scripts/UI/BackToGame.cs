using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToGame : MonoBehaviour
{
    public GameObject exitPanel;

    public void Back()
    {
        exitPanel.SetActive(false);
    }
}
