using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragonfly : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    public SpaceObject spaceObject;

    public GameObject mine;

    public int waypointCount;
    private Vector3[] waypoints;
    private int indexWaypoint;

    public Vector2 leftUpCorner;
    public Vector2 rightDownCorner;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GenerateWayPoint();
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

    void Update()
    {
        Move();
    }

    void Move()
    {
        if(indexWaypoint < waypoints.Length)
        {
            var heading = waypoints[indexWaypoint] - transform.position;
            var distance = heading.magnitude;

            Vector3 directionMove = heading / distance;
            rb.velocity = directionMove * speed * spaceObject.speedMultiplier;

            if(distance <= 0.1f)
            {
                PlantMine();
                indexWaypoint++;
            }
        }
        else
        {
            rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);
        }
    }

    void PlantMine()
    {
        GameObject mine = Instantiate(this.mine, transform.position, Quaternion.identity);
        mine.GetComponent<Mine>().spaceObject = spaceObject;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
