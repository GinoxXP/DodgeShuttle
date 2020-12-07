using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rusher : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float speedAiming;

    Rigidbody2D rb;
    [SerializeField] SpaceObject spaceObject;

    Transform player;

    [SerializeField] float timeFireDelay;
    float timerFireDelay;
    bool isFire;

    [SerializeField] GameObject bullet;

    [SerializeField] int fireCountBeforeRush;
    int fireCounter;

    bool isRush;

    [SerializeField] Drop drop;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);

        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    void Update()
    {
        if(!isRush)
            Move();
        else
            Rush();
    }

    void Move()
    {
        Aim();

        if(!isFire)
            Fire();

        if(isFire)
            timerFireDelay += Time.deltaTime;

        if(timerFireDelay >= timeFireDelay)
        {
            timerFireDelay = 0;
            isFire = false;
        }

        if(fireCounter >= fireCountBeforeRush)
            isRush = true;
    }

    void Aim()
    {
        if(player != null)
            rb.velocity = new Vector2(rb.velocity.x,
                                    (player.position.y > transform.position.y ? speedAiming : -speedAiming) * spaceObject.speedMultiplier);
        else
            rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    void Rush()
    {
        if(player == null)
            return;
            
        var heading = player.position - transform.position;
        var distance = heading.magnitude;

        if(distance >= 2)
        {
            Vector3 directionMove = heading / distance;
            rb.velocity = directionMove * speed * spaceObject.speedMultiplier * 3;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().speedMultiplier = spaceObject.speedMultiplier;
        isFire = true;
        fireCounter++;
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
