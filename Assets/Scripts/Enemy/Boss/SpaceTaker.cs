using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceTaker : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] SpaceObject spaceObject;

    Rigidbody2D rb;

    [SerializeField] int hp;

    [Space]
    [SerializeField] ParticleSystem firePoint1;
    [SerializeField] ParticleSystem firePoint2;
    [SerializeField] ParticleSystem firePoint3;
    [SerializeField] ParticleSystem firePoint4;
    [SerializeField] ParticleSystem firePoint5;

    [Space]
    [SerializeField] GameObject asteroid;
    [SerializeField] float timeDropAsteroidDelay;
    [SerializeField] Vector3 asteroidSpawnOffset;

    [Space]
    [SerializeField] GameObject swarmShield;
    [SerializeField] float timeSwarmShieldDelay;

    [Space]
    [SerializeField] GameObject swarm;
    [SerializeField] float timeSwarmDelay;
    [SerializeField] Vector3 swarmSpawnOffset;
    [SerializeField] int hpSwarmSpawn;

    [Space]
    [SerializeField] int waypointCount;
    Vector3[] waypoints;
    int indexWaypoint;

    [SerializeField] Vector2 leftUpCorner;
    [SerializeField] Vector2 rightDownCorner;

    [SerializeField] Drop drop;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GenerateWayPoint();

        StartCoroutine(SpawnAsteroid());
        StartCoroutine(MakeSwarmShield());
        StartCoroutine(SpawnSwarm());
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

    IEnumerator SpawnAsteroid()
    {
        while(true)
        {
            for(int i = 0; i < 7; i++)
            {
                float randomSpeedY = Random.Range(-3.0f, 3.0f);

                GameObject asteroid = Instantiate(this.asteroid,
                                                  transform.position + asteroidSpawnOffset,
                                                  Quaternion.identity);

                Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
                asteroidComponent.speed = new Vector3(-3 * spaceObject.speedMultiplier,
                                                      randomSpeedY * spaceObject.speedMultiplier);
            }

            yield return new WaitForSeconds(timeDropAsteroidDelay);
        }
    }

    IEnumerator MakeSwarmShield()
    {
        while(true)
        {
            GameObject swarmShield = Instantiate(this.swarmShield,
                                                 transform.position,
                                                 Quaternion.identity);

            swarmShield.GetComponent<SwarmShield>().speed *= spaceObject.speedMultiplier;

            swarmShield.transform.parent = transform;

            yield return new WaitForSeconds(timeSwarmDelay);
        }
    }

    IEnumerator SpawnSwarm()
    {
        while(true)
        {
            if(hp < hpSwarmSpawn)
            {
                GameObject swarm = Instantiate(this.swarm,
                                                  transform.position + swarmSpawnOffset,
                                                  Quaternion.identity);

                for(int i = 0; i < swarm.transform.childCount; i++)
                {
                    GameObject swarmUnit = swarm.transform.GetChild(i).gameObject;

                    swarmUnit.GetComponent<SpaceObject>().speedMultiplier = spaceObject.speedMultiplier;
                }

                yield return new WaitForSeconds(timeSwarmDelay);
            }

            yield return null;
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

    void SetStatus()
    {
        if(hp == 80)
            firePoint1.Play();

        if(hp == 60)
            firePoint2.Play();

        if(hp == 40)
            firePoint3.Play();

        if(hp == 20)
            firePoint4.Play();

        if(hp == 10)
            firePoint5.Play();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            Destroy(col.gameObject);

            hp--;
            SetStatus();

            if(hp <= 0)
            {
                drop.DropItem(spaceObject.speedMultiplier);
                Destroy(gameObject);
            }
        }
    }
}
