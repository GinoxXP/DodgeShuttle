using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitMenu : MonoBehaviour
{
    public GameObject exitPanel;

    public void Exit()
    {
        exitPanel.SetActive(!exitPanel.activeSelf);    
    }
}
