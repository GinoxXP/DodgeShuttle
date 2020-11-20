using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    public float speedMultiplier;

    public DropItem[] dropItems;

    public void Drop()
    {
        float randValue = Random.Range(0.0f, 100.0f);
        for(int i = 0; i < dropItems.Length; i++)
        {
            if(dropItems[i].dropPercent >= randValue)
            {
                Instantiate(dropItems[i].item, transform.position, Quaternion.identity);
            }
        }
    }
}

[System.Serializable]
public struct DropItem
{
    public GameObject item;
    [Range(0,100)]
    public float dropPercent;
}
