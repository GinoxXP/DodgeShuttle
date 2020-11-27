using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public DropItem[] dropItems;

    public void DropItem(float speedMultiplier)
    {
        float randValue = Random.Range(0.0f, 1.0f);
        for(int i = 0; i < dropItems.Length; i++)
        {
            if(dropItems[i].dropFrequency >= randValue)
            {
                GameObject drop = Instantiate(dropItems[i].item, transform.position, Quaternion.identity);
                drop.GetComponent<Rigidbody2D>().velocity = new Vector2(-2 * speedMultiplier,0);
                return;
            }
        }
    }
}

[System.Serializable]
public struct DropItem
{
    public GameObject item;
    [Range(0,1)]
    public float dropFrequency;
}
