using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] SpaceObject spaceObject;

    public float bulletSpeed;

    public void Fire()
    {
        GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().speedMultiplier = spaceObject.speedMultiplier;
        bullet.GetComponent<Bullet>().velocity = -transform.right * bulletSpeed;
    }
}
