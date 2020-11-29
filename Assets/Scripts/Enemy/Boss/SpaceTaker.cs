using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTaker : MonoBehaviour
{
    public float speed;

    public SpaceObject spaceObject;

    private Rigidbody2D rb;

    public int hp;
    [Space]
    public ParticleSystem firePoint1;
    public ParticleSystem firePoint2;
    public ParticleSystem firePoint3;
    public ParticleSystem firePoint4;
    public ParticleSystem firePoint5;

    [Space]
    public GameObject asteroid;
    public float timeDropAsteroidDelay;
    private float timerDropAsteroidDelay;
    private bool isDropAsteroid;
    public Vector3 asteroidSpawnOffset;

    [Space]
    public GameObject swarmShield;
    public float timeSwarmShieldDelay;
    private float timerSwarmShieldDelay;
    private bool isSwarmShield;

    [Space]
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
        Move();

        TryDropAsteroid();
        TryMakeSwarmShield();
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

    void TryDropAsteroid()
    {
        if(!isDropAsteroid)
            DropAsteroid();

        if(isDropAsteroid)
            timerDropAsteroidDelay += Time.deltaTime;

        if(timerDropAsteroidDelay >= timeDropAsteroidDelay)
        {
            timerDropAsteroidDelay = 0;
            isDropAsteroid = false;
        }
    }

    void DropAsteroid()
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

        isDropAsteroid = true;

    }

    void TryMakeSwarmShield()
    {
        if(!isSwarmShield)
            MakeSwarmShield();

        if(isSwarmShield)
            timerSwarmShieldDelay += Time.deltaTime;

        if(timerSwarmShieldDelay >= timeSwarmShieldDelay)
        {
            timerSwarmShieldDelay = 0;
            isSwarmShield = false;
        }
    }

    void MakeSwarmShield()
    {
        GameObject swarmShield = Instantiate(this.swarmShield,
                                             transform.position,
                                             Quaternion.identity);

        swarmShield.GetComponent<SwarmShield>().speed *= spaceObject.speedMultiplier;

        swarmShield.transform.parent = transform;

        isSwarmShield = true;
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
            if(hp <= 0)
            {
                drop.DropItem(spaceObject.speedMultiplier);
                Destroy(gameObject);
            }

            SetStatus();
        }
    }
}
