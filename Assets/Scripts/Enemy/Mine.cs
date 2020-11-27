using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public bool isActive;

    public float speed;

    private Rigidbody2D rb;
    public SpaceObject spaceObject;

    public GameObject bullet;

    public Weapon[] weapons;

    public int timeFireDelay;
    private float timerFireDelay;
    private bool isFire;

    public float time;

    public int rotationDegreeInSecond;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);
    }

    void Update()
    {
        if(isActive)
        {
            transform.Rotate(transform.forward, rotationDegreeInSecond * Time.deltaTime);

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
    }

    private void Fire()
    {
        // GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        // bullet.GetComponent<Bullet>().speedMultiplier = spaceObject.speedMultiplier;

        for(int i = 0; i < weapons.Length; i++)
            weapons[i].Fire();
        isFire = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            //spaceObject.Drop();
            isActive = true;
            Destroy(col.gameObject);
        }
    }
}
