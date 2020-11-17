using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnedObject[] spawnedObjects;

    private float time;

    public float divider;
    private float dividerTime;

    public float hightMultiplier;

    public float speedMultiplier;

    public Transform point1;
    public Transform point2;

    void Update()
    {
        time += Time.deltaTime;

        if(time - dividerTime >= divider)
        {
            dividerTime = time;
            if(divider > 0.5f)
            {
                divider /= hightMultiplier;
                speedMultiplier *= hightMultiplier;
            }

            Spawn();
        }
    }

    void Spawn()
    {
        for(int i = 0; i < spawnedObjects.Length; i++)
        {
            float y = Random.Range(point2.position.y, point1.position.y);
            Vector2 spawnPosition = new Vector2(point1.position.x, y);

            GameObject obj = Instantiate(spawnedObjects[i].obj, spawnPosition, Quaternion.identity);
            obj.GetComponent<SpaceObject>().speedMultiplier = speedMultiplier;

            break;
        }
    }
}

[System.Serializable]
public struct SpawnedObject
{
    public GameObject obj;
    public float rare;
}
