using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mine : MonoBehaviour
{
    [SerializeField] bool isActive;

    [SerializeField] float speed;

    Rigidbody2D rb;
    public SpaceObject spaceObject;

    [SerializeField] GameObject bullet;

    [SerializeField] Weapon[] weapons;

    [SerializeField] int timeFireDelay;

    [SerializeField] float time;

    [SerializeField] int rotationDegreeInSecond;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed * spaceObject.speedMultiplier, 0);

        for(int i = 0; i < weapons.Length; i++)
            weapons[i].bulletSpeed *= spaceObject.speedMultiplier;
    }

    void Update()
    {
        if(isActive)
        {
            transform.Rotate(transform.forward, rotationDegreeInSecond * Time.deltaTime);
        }
    }

    IEnumerator Fire()
    {
        while(true)
        {
            if(isActive)
            {
                for(int i = 0; i < weapons.Length; i++)
                    weapons[i].Fire();

                yield return new WaitForSeconds(timeFireDelay);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.TryGetComponent(out Bullet bullet))
        {
            isActive = true;

            if(col.tag == "Bullet")
                Destroy(col.gameObject);
        }
    }
}
