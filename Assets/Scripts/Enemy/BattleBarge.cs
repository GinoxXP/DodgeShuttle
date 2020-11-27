using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBarge : MonoBehaviour
{
    public float speed;

    public SpaceObject spaceObject;

    private Rigidbody2D rb;

    public int hp;

    public Turret turret1;
    public Turret turret2;

    public float timeFireDelay;
    private float timerFireDelay;
    private bool isFire;

    public int waypointCount;
    private Vector3[] waypoints;
    private int indexWaypoint;

    public Vector2 leftUpCorner;
    public Vector2 rightDownCorner;

    public Drop drop;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GenerateWayPoint();
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
        if(hp == 20)
            turret1.MakeBroken();

        if(hp == 10)
            turret2.MakeBroken();
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
