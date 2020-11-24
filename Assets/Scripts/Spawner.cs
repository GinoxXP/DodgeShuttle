using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public DifficultLevel[] difficultLevels;
    private DifficultLevel currentDifficultLevel;

    private List<Transform> spawnedObjects = new List<Transform>();
    public int guaranteedDestroyDistance;

    public Transform pointUpBorder;
    public Transform pointDownBorder;

    public float speedMultiplier;
    public float speedMultiplierStart;

    public float spawnDelayTime;
    private float spawnDelayTimer;
    private bool isSpawned;

    public float distance;

    void Start()
    {
        currentDifficultLevel = difficultLevels[0];
    }

    void Update()
    {
        DestroyObjects();

        if(!currentDifficultLevel.isBossFight)
        {
            ComputeSpeedMultiplier();
            Spawn();

            distance += Time.deltaTime;
        }

        if(isSpawned)
        {
            spawnDelayTimer += Time.deltaTime;

            if(spawnDelayTimer >= spawnDelayTime)
            {
                isSpawned = false;
                spawnDelayTimer = 0;
            }
        }
    }

    void Spawn()
    {
        if(!isSpawned)
        {
            isSpawned = true;

            float randomValue = Random.Range(0.0f, 1.0f);

            for(int i = 0; i < currentDifficultLevel.spawningObjects.Length; i++)
            {
                var spawningObject = currentDifficultLevel.spawningObjects[i];
                if(spawningObject.frequencySpawn >= randomValue)
                {
                    float yRandom = Random.Range(pointUpBorder.position.y,
                                                 pointDownBorder.position.y);

                    Vector3 spawnPosition = new Vector3(pointUpBorder.position.x,
                                                        yRandom);

                    GameObject spawnedObject = Instantiate(spawningObject.spawningObject,
                                                           spawnPosition,
                                                           Quaternion.identity);

                   spawnedObject.GetComponent<SpaceObject>().speedMultiplier = speedMultiplier;

                   spawnedObjects.Add(spawnedObject.transform);
                }
            }
        }
    }

    void ComputeSpeedMultiplier()
    {
        speedMultiplier = (distance / 100) + speedMultiplierStart;
    }

    void DestroyObjects()
    {
        for(int i = 0; i < spawnedObjects.Count; i++)
        {
            if(spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                break;
            }
            
            if(Vector3.Distance(transform.position, spawnedObjects[i].position)
              >= guaranteedDestroyDistance)
            {
                Destroy(spawnedObjects[i].gameObject);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}

[System.Serializable]
public struct SpawningObject
{
    public GameObject spawningObject;
    [Range(0,1)]
    public float frequencySpawn;
}

[System.Serializable]
public struct DifficultLevel
{
    public int startDistance;
    public SpawningObject[] spawningObjects;
    public bool isBossFight;
}
