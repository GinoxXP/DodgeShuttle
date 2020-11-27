using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    public Text text;
    public Spawner spawner;

    public bool isPause;

    void Update()
    {
        if(!isPause)
        {
            text.text = System.Math.Round(spawner.distance, 2).ToString();
        }
    }
}
