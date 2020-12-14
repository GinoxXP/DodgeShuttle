using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Dragonfly : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody2D rb;
    [SerializeField] SpaceObject spaceObject;

    [SerializeField] GameObject mine;

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

            if(distance <= 0.3f)
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
            drop.DropItem(spaceObject.speedMultiplier);
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
