using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeKit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();

            if(player.nextGenerationShuttle != null)
            {
                GameObject newShuttle = player.nextGenerationShuttle;
                GameObject oldShuttle = player.gameObject;

                oldShuttle.SetActive(false);
                
                newShuttle.SetActive(true);
                newShuttle.transform.position = oldShuttle.transform.position;
            }

            Destroy(gameObject);
        }
    }
}
