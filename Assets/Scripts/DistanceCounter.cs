using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    public Text text;
    private float time;

    public bool isPause;

    void Update()
    {
        if(!isPause)
        {
            time += Time.deltaTime;
            text.text = System.Math.Round(time, 2).ToString();
        }
    }
}
