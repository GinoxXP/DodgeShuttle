using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float speed;

    Vector2 velocity;
    Rigidbody2D rb;

    [SerializeField] float fireDelayTime;
    float fireDelayTimer;
    bool isFire;

    [SerializeField] SpaceObject spaceObject;

    public bool isBroken;
    [SerializeField] ParticleSystem particleSystem;

    public bool isImmunity;
    [SerializeField] GameObject shield;
    [SerializeField] float immunityTime;
    float immunityTimer;

    public GameObject nextGenerationShuttle;

    [SerializeField] Weapon[] weapons;

    bool isFireReady;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();

        if(isFireReady)
        {
            if(!isFire)
                Fire();
        }

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

    void Move()
    {
        if(velocity == null)
            velocity = new Vector2(0, 0);

        rb.velocity = velocity.normalized * speed * spaceObject.speedMultiplier;
    }

    void Fire()
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
        if(context.started)
            isFireReady = true;
        if(context.canceled)
            isFireReady = false;
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
