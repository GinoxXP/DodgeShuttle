using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;

    private Vector2 velocity;
    private Rigidbody2D rb;

    public GameObject bullet;

    public float fireDelayTime;
    private float fireDelayTimer;
    private bool isFire;

    public SpaceObject spaceObject;

    public bool isBroken;
    public ParticleSystem particleSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();

        if(isFire)
        {
            fireDelayTimer += Time.deltaTime;

            if(fireDelayTimer >= fireDelayTime)
            {
                fireDelayTimer = 0;
                isFire = false;
            }
        }
    }

    private void Move()
    {
        if(velocity == null)
            velocity = new Vector2(0, 0);

        rb.velocity = velocity.normalized * speed * spaceObject.speedMultiplier;
    }

    private void Fire()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
        isFire = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
            if(!isFire)
                Fire();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            Destroy(col.gameObject);

            if(isBroken)
                Destroy(gameObject);
            if(!isBroken)
            {
                isBroken = true;
                particleSystem.Play();
            }
        }
    }
}
