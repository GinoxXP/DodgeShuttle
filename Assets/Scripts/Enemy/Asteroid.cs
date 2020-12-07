using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    public Vector3 speed;

    Rigidbody2D rb;
    [SerializeField] SpaceObject spaceObject;

    [SerializeField] Drop drop;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed * spaceObject.speedMultiplier;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            drop.DropItem(spaceObject.speedMultiplier);
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
