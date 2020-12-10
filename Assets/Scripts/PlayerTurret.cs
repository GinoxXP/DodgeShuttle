using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class PlayerTurret : MonoBehaviour
{
    GameObject[] enemies;
    Transform target;

    Weapon weapon;

    [Space]
    [SerializeField] float fireDelayTime;
    float fireDelayTimer;
    bool isFire;

    void Start()
    {
        weapon = GetComponent<Weapon>();
    }

    void Update()
    {
        if(target == null)
            SetTarget();

        if(enemies.Length > 0)
            Aim();


        if(!isFire)
        {
            if(target != null)
                Fire();
        }
        else
        {
            fireDelayTimer += Time.deltaTime;
        }

        if(fireDelayTimer >= fireDelayTime)
        {
            fireDelayTimer = 0;
            isFire = false;
        }
    }

    void SetTarget()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float nearDistance = Mathf.Infinity;
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].TryGetComponent(out Mine mine))
                break;

            if(enemies[i].TryGetComponent(out Bullet bullet))
                break;

            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if(distance < nearDistance)
            {
                nearDistance = distance;
                target = enemies[i].transform;
            }
        }
    }

    void Aim()
    {
        if(target == null)
            return;
            
        Vector3 diff = target.position - transform.position;
        diff.Normalize();
        float rot_Z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler (0f, 0f, rot_Z);
    }

    void Fire()
    {
        isFire = true;
        weapon.Fire();
    }
}
