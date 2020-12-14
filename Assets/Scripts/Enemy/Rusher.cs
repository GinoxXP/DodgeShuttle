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

    [SerializeField] GameObject bullet;

    [SerializeField] int fireCountBeforeRush;

    bool isRush;

    [SerializeField] Drop drop;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);

        SetTarget();

        StartCoroutine(Fire());
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
    }

    void Aim()
    {
        if(player == null || !player.gameObject.activeSelf)
            SetTarget();
            
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

        if(player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier * 6, 0);
            return;
        }

        var heading = player.position - transform.position;
        var distance = heading.magnitude;

        if(distance >= 2)
        {
            Vector3 directionMove = heading / distance;
            rb.velocity = directionMove * speed * spaceObject.speedMultiplier * 3;
        }
    }

    void SetTarget()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    IEnumerator Fire()
    {
        for(int i = 0; i < fireCountBeforeRush; i++)
        {
            GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().speedMultiplier = spaceObject.speedMultiplier;

            yield return new WaitForSeconds(timeFireDelay);
        }

        isRush = true;
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
