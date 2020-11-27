using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public SpaceObject spaceObject;

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.
                GetChild(i).
                GetComponent<SpaceObject>().
                speedMultiplier = spaceObject.speedMultiplier;
        }
    }
}
