using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float speedAiming;

    Rigidbody2D rb;
    [SerializeField] SpaceObject spaceObject;

    Transform player;

    [SerializeField] float timeFireDelay;
    float timer;
    bool isFire;

    [SerializeField] GameObject bullet;

    [SerializeField] float lifeTime;
    float lifeTimer;
    bool isAlive = true;

    [SerializeField] Drop drop;

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
            Aim();

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
            rb.velocity = new Vector2(speed*3 * spaceObject.speedMultiplier, 0);
    }

    void Aim()
    {
        if(player != null)
            rb.velocity = new Vector2(rb.velocity.x,
                                    (player.position.y > transform.position.y ? speedAiming : -speedAiming) * spaceObject.speedMultiplier); 
        else
            rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    void Fire()
    {
        GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().speedMultiplier = spaceObject.speedMultiplier;
        isFire = true;
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
