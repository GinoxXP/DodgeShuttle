using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexon : MonoBehaviour
{
    public float speed;

    public SpaceObject spaceObject;

    private Rigidbody2D rb;

    public int hp;

    [Space]
    public Turret turret1;
    public Turret turret2;

    [Space]
    public float timeFireDelay;
    private float timerFireDelay;
    private bool isFire;

    [Space]
    public GameObject torpedo;
    public float timeTorpedoDelay;
    private float timerTorpedoDelay;
    private bool isTorpedo;

    [Space]
    public int waypointCount;
    private Vector3[] waypoints;
    private int indexWaypoint;

    [Space]
    public Vector2 leftUpCorner;
    public Vector2 rightDownCorner;

    public Drop drop;

    [Space]
    public float timeDischargeDelay;
    private float timerDischargeDelay;
    private bool isDischarge;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GenerateWayPoint();
    }

    void Update()
    {
        TryFire();
        TryThrowTorpedo();

        if(hp < 100)
            TryDischarge();

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

    void TryFire()
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
    }

    void Fire()
    {
        isFire = true;

        turret1.Fire();
        turret2.Fire();
    }

    void TryThrowTorpedo()
    {
        if(!isTorpedo)
            ThrowTorpedo();

        if(isTorpedo)
            timerTorpedoDelay += Time.deltaTime;

        if(timerTorpedoDelay >= timeTorpedoDelay)
        {
            timerTorpedoDelay = 0;
            isTorpedo = false;
        }
    }

    void ThrowTorpedo()
    {
        isTorpedo = true;

        Instantiate(torpedo, transform.position, Quaternion.identity);
    }

    void TryDischarge()
    {
        if(!isDischarge)
            Discharge();

        if(isDischarge)
            timerDischargeDelay += Time.deltaTime;

        if(timerDischargeDelay >= timeDischargeDelay)
        {
            timerDischargeDelay = 0;
            isDischarge = false;
        }
    }

    void Discharge()
    {
        isDischarge = true;

        for(int i = -3; i < 3; i++)
        {
            Instantiate(torpedo, transform.position + new Vector3(0, i), Quaternion.identity);
        }
    }

    void SetStatus()
    {
        if(hp == 70)
            turret1.MakeBroken();

        if(hp == 20)
        {
            turret2.MakeBroken();
            timeTorpedoDelay = 2;

            timeDischargeDelay = 5;
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
