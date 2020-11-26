using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immunity : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();

            player.isImmunity = true;

            Destroy(gameObject);
        }
    }
}
