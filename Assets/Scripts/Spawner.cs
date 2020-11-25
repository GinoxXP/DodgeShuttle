using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public DifficultLevel[] difficultLevels;
    private DifficultLevel currentDifficultLevel;
    private int indexDifficultLevel;

    public Transform pointUpBorder;
    public Transform pointDownBorder;

    public float speedMultiplier;
    public float speedMultiplierStart;

    public float spawnDelayTime;
    private float spawnDelayTimer;
    private bool isSpawned;

    public float distance;

    private bool isBossSpawned;

    void Start()
    {
        currentDifficultLevel = difficultLevels[0];
    }

    void Update()
    {
        SetDifficultLevel();
        if(!currentDifficultLevel.isBossFight)
        {
            ComputeSpeedMultiplier();
            Spawn();

            distance += Time.deltaTime;
        }
        else
        {
            if(!isBossSpawned)
            {
                Spawn();
                isBossSpawned = true;
            }
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

    void SetDifficultLevel()
    {
        try{
            if(difficultLevels[indexDifficultLevel+1].startDistance < distance)
            {
                currentDifficultLevel = difficultLevels[indexDifficultLevel+1];
                indexDifficultLevel++;
            }
        }
        catch(System.IndexOutOfRangeException){}
    }

    void Spawn()
    {
        if(!isSpawned || currentDifficultLevel.isBossFight)
        {
            isSpawned = true;

            for(int i = 0; i < currentDifficultLevel.spawningObjects.Length; i++)
            {
                float randomValue = Random.Range(0.0f, 1.0f);
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

                   return;
                }
            }
        }
    }

    void ComputeSpeedMultiplier()
    {
        speedMultiplier = (distance / 100) + speedMultiplierStart;
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
