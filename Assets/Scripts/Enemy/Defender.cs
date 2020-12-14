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

    [SerializeField] GameObject bullet;

    [SerializeField] float lifeTime;
    bool isAlive = true;

    [SerializeField] Drop drop;

    Coroutine fireCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);

        SetTarget();

        fireCoroutine = StartCoroutine(Fire());
        StartCoroutine(GoHome());
    }

    void Update()
    {
        if(isAlive)
            Aim();
        else
            rb.velocity = new Vector2(speed*3 * spaceObject.speedMultiplier, 0);
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

    void SetTarget()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    IEnumerator Fire()
    {
        while(true)
        {
            GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().speedMultiplier = spaceObject.speedMultiplier;

            yield return new WaitForSeconds(timeFireDelay);
        }
    }

    IEnumerator GoHome()
    {
        yield return new WaitForSeconds(lifeTime);
        isAlive = false;

        StopCoroutine(fireCoroutine);
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
