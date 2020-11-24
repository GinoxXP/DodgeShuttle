using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmUnit : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    public SpaceObject spaceObject;

    private float time;
    public float timeDelay;

    public float amplitudeMultiplier;

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
            //spaceObject.Drop();
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
