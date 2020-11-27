using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;

    private Vector2 velocity;
    private Rigidbody2D rb;

    public float fireDelayTime;
    private float fireDelayTimer;
    private bool isFire;

    public SpaceObject spaceObject;

    public bool isBroken;
    public ParticleSystem particleSystem;

    public bool isImmunity;
    public GameObject shield;
    public float immunityTime;
    private float immunityTimer;

    public GameObject nextGenerationShuttle;

    public Weapon[] weapons;


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

        if(isImmunity)
            ComputeImmunity();
    }

    private void Move()
    {
        if(velocity == null)
            velocity = new Vector2(0, 0);

        rb.velocity = velocity.normalized * speed * spaceObject.speedMultiplier;
    }

    private void Fire()
    {
        for(int i = 0; i < weapons.Length; i++)
            weapons[i].Fire();

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

    public void SetStatus()
    {
        if(isBroken)
            particleSystem.Play();
        else
            particleSystem.Stop();
    }

    void ComputeImmunity()
    {
        immunityTimer += Time.deltaTime;

        shield.SetActive(true);

        if(immunityTimer >= immunityTime)
        {
            isImmunity = false;
            immunityTimer = 0;

            shield.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            Destroy(col.gameObject);

            if(!isImmunity)
            {
                if(isBroken)
                {
                    Destroy(gameObject);
                }
                if(!isBroken)
                {
                    isBroken = true;
                    SetStatus();
                }
            }
        }
    }
}
