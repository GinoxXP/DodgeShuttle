using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBarge : MonoBehaviour
{
    float speed;

    [SerializeField] float speedFirstPhase;
    [SerializeField] float speedSecondPhase;
    [SerializeField] float speedThirdPhase;

    [SerializeField] SpaceObject spaceObject;

    Rigidbody2D rb;

    [SerializeField] int hp;

    [Space]
    [SerializeField] Turret turret1;
    [SerializeField] Turret turret2;

    [Space]
    [SerializeField] float timeFireDelay;
    float timerFireDelay;
    bool isFire;

    [Space]
    [SerializeField] int waypointCount;
    Vector3[] waypoints;
    int indexWaypoint;

    [Space]
    [SerializeField] Vector2 leftUpCorner;
    [SerializeField] Vector2 rightDownCorner;

    [Space]
    [SerializeField] Drop drop;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GenerateWayPoint();

        speed = speedFirstPhase;
    }

    void Update()
    {
        if(!isFire)
            Fire();

        if(isFire)
            timerFireDelay += Time.deltaTime;

        if(timerFireDelay >= timeFireDelay)
        {
            timerFireDelay = 0;
            isFire = false;
        }

        Move();
    }

    void Move()
    {
        var heading = waypoints[indexWaypoint] - transform.position;
        var distance = heading.magnitude;

        Vector3 directionMove = heading / distance;
        rb.velocity = directionMove * speed * spaceObject.speedMultiplier;

        if(distance <= 0.1f)
        {
            indexWaypoint++;
            if(indexWaypoint >= waypoints.Length)
                indexWaypoint = 0;
        }
    }

    void GenerateWayPoint()
    {
        waypoints = new Vector3[waypointCount];
        for(int i = 0; i < waypointCount; i++)
        {
            float x = Random.Range(leftUpCorner.x, rightDownCorner.x);
            float y = Random.Range(leftUpCorner.y, rightDownCorner.y);

            waypoints[i] = new Vector3(x,y);
        }
    }

    void Fire()
    {
        isFire = true;

        turret1.Fire();
        turret2.Fire();
    }

    void SetStatus()
    {
        if(hp <= 20)
        {
            turret1.MakeBroken();

            speed = speedSecondPhase;
        }

        if(hp <= 10)
        {
            turret2.MakeBroken();

            speed = speedThirdPhase;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            Destroy(col.gameObject);

            hp--;
            if(hp <= 0)
            {
                drop.DropItem(spaceObject.speedMultiplier);
                Destroy(gameObject);
            }

            SetStatus();
        }
    }
}
