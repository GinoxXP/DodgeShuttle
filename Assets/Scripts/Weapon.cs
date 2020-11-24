using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public SpaceObject spaceObject;

    public int bulletSpeed;

    public void Fire()
    {
        GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().speedMultiplier = spaceObject.speedMultiplier;
        bullet.GetComponent<Bullet>().velocity = -transform.right * bulletSpeed;
    }
}
