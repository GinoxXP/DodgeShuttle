using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SwarmUnit : MonoBehaviour
{
    [SerializeField] float speed;

    [Space]
    Rigidbody2D rb;
    [SerializeField] SpaceObject spaceObject;

    [Space]
    float time;
    [SerializeField] float timeDelay;

    [Space]
    [SerializeField] float amplitudeMultiplier;

    [Space]
    [SerializeField] Drop drop;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);
    }

    void Update()
    {
        time += Time.deltaTime;

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sin((time+timeDelay) * spaceObject.speedMultiplier*2) * amplitudeMultiplier);
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
