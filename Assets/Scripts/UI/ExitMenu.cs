using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitMenu : MonoBehaviour
{
    public GameObject exitPanel;

    public void OnExit(InputAction.CallbackContext context)
    {
        if(context.performed)
            exitPanel.SetActive(!exitPanel.activeSelf);
    }
}
