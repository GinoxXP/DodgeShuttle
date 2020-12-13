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

    [SerializeField] SpaceObject spaceObject;

    public bool isBroken;
    [SerializeField] ParticleSystem particleSystem;

    public bool isImmunity;
    [SerializeField] GameObject shield;
    [SerializeField] float immunityTime;

    public GameObject nextGenerationShuttle;

    [SerializeField] Weapon[] weapons;

    bool isFireReady;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(Fire());
        StartCoroutine(Immunity());
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if(velocity == null)
            velocity = new Vector2(0, 0);

        rb.velocity = velocity.normalized * speed * spaceObject.speedMultiplier;
    }

    IEnumerator Fire()
    {
        while(true)
        {
            if(isFireReady)
            {
                for(int i = 0; i < weapons.Length; i++)
                    weapons[i].Fire();

                yield return new WaitForSeconds(fireDelayTime);
            }
			yield return null;
        }
    }

    IEnumerator Immunity()
    {
        while(true)
        {
            if(isImmunity)
            {
                shield.SetActive(true);

                yield return new WaitForSeconds(immunityTime);

                shield.SetActive(false);

                isImmunity = false;
            }
			yield return null;
        }
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
