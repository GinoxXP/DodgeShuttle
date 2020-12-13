using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Hexon : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] SpaceObject spaceObject;

    private Rigidbody2D rb;

    [SerializeField] int hp;

    [Space]
    [SerializeField] Turret turret1;
    [SerializeField] Turret turret2;

    [Space]
    [SerializeField] float timeFireDelay;

    [Space]
    [SerializeField] GameObject torpedo;
    [SerializeField] float timeTorpedoDelay;

    [Space]
    [SerializeField] int waypointCount;
    Vector3[] waypoints;
    int indexWaypoint;

    [Space]
    [SerializeField] Vector2 leftUpCorner;
    [SerializeField] Vector2 rightDownCorner;

    [SerializeField] Drop drop;

    [Space]
    [SerializeField] float timeDischargeDelay;
    [SerializeField] int hpDischarge;

    [Space]
    [SerializeField] int hpBrokenTurret1;
    [SerializeField] int hpBrokenTurret2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GenerateWayPoint();

        StartCoroutine(Fire());
        StartCoroutine(ThrowTorpedo());
        StartCoroutine(Discharge());
    }

    void Update()
    {
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

    IEnumerator Fire()
    {
        while(true)
        {
            turret1.Fire();
            turret2.Fire();

            yield return new WaitForSeconds(timeFireDelay);
        }
    }

    IEnumerator ThrowTorpedo()
    {
        while(true)
        {
            Instantiate(torpedo, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(timeTorpedoDelay);
        }
    }

    IEnumerator Discharge()
    {
        while(true)
        {
            if(hp < hpDischarge)
            {
                for(int i = -3; i < 3; i++)
                    Instantiate(torpedo, transform.position + new Vector3(0, i), Quaternion.identity);

                yield return new WaitForSeconds(timeDischargeDelay);
            }
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
