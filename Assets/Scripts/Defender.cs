using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    public float speed;
    public float speedAiming;

    private Rigidbody2D rb;
    public SpaceObject spaceObject;

    private Transform player;

    public float timeFireDelay;
    private float timer;
    private bool isFire;

    public GameObject bullet;

    public float lifeTime;
    private float lifeTimer;
    private bool isAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);

        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    void Update()
    {
        if(isAlive)
        {
            if(player != null)
                rb.velocity = new Vector2(rb.velocity.x, player.position.y > transform.position.y ? speedAiming : -speedAiming);
            else
                rb.velocity = new Vector2(rb.velocity.x, 0);

            if(!isFire)
                Fire();

            if(isFire)
                timer += Time.deltaTime;

            if(timer >= timeFireDelay)
            {
                timer = 0;
                isFire = false;
            }

            lifeTimer += Time.deltaTime;

            if(lifeTimer >= lifeTime)
                isAlive = false;
        }
        else
            rb.velocity = new Vector2(speed*3, 0);
    }

    private void Fire()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
        isFire = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
