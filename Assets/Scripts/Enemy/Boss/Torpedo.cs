﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Torpedo : MonoBehaviour
{
    Transform player;
    Rigidbody2D rb;

    [SerializeField] float speed;
    [SerializeField] float speedBoost;

    [Space]
    [SerializeField] float boostDistance;

    [Space]
    [SerializeField] bool isBoost;

    [Space]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite boostedTorpedo;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 10);
    }

    void Update()
    {
        if(player != null)
            Aim();

        Move();
    }

    void Aim()
    {
        Vector3 diff = player.position - transform.position;
        diff.Normalize();
        float rot_Z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler (0f, 0f, rot_Z - 180);
    }

    void Move()
    {
        if(player == null)
            return;
            
        float distance = Vector3.Distance(player.position, transform.position);

        if(distance <= boostDistance)
        {
            isBoost = true;
        }

        if(isBoost)
        {
            Vector3 directionMove = rb.velocity;
            directionMove.Normalize();
            rb.velocity = directionMove * speedBoost;

            spriteRenderer.sprite = boostedTorpedo;
        }
        else
        {
            Vector3 diff = player.position - transform.position;
            diff.Normalize();
            rb.velocity = diff * speed;
        }

    }
}
