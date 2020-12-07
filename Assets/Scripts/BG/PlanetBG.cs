using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlanetBG : MonoBehaviour
{
    [SerializeField] Vector3 velocity;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
