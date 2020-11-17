using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnedObjects;

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
        int indexSpawned = Random.Range(0, spawnedObjects.Length);

        float y = Random.Range(point2.position.y, point1.position.y);
        Vector2 spawnPosition = new Vector2(point1.position.x, y);

        GameObject obj = Instantiate(spawnedObjects[indexSpawned],
                                     spawnPosition,
                                     Quaternion.identity);

        obj.GetComponent<SpaceObject>().speedMultiplier = speedMultiplier;
    }
}

[System.Serializable]
public struct SpawnedObject
{
    public GameObject obj;
}
