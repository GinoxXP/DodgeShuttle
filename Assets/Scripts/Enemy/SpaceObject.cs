﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    public float speedMultiplier;

    public bool isGroup;

    void Update()
    {
        if(isGroup && transform.childCount == 0)
            Destroy(gameObject);
    }
}
