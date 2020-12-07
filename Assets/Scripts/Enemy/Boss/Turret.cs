using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    Transform player;

    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] bool isBroken;

    [Space]
    [SerializeField] Weapon weapon;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    void Update()
    {
        if(player != null && !isBroken)
            Aim();
    }

    void Aim()
    {
        Vector3 diff = player.position - transform.position;
        diff.Normalize();
        float rot_Z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler (0f, 0f, rot_Z - 180);
    }

    public void Fire()
    {
        if(!isBroken)
            weapon.Fire();
    }

    public void MakeBroken()
    {
        isBroken = true;
        particleSystem.Play();
    }
}
