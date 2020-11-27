using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public Transform player;
    public GameObject gameOverPanel;
    public Spawner spawner;

    void Update()
    {
        bool gameOver = true;

        for(int i = 0; i < player.childCount; i++)
        {
            if(player.GetChild(i).gameObject.activeSelf)
                gameOver = false;
        }

        if(gameOver)
        {
            spawner.isPause = true;
            gameOverPanel.SetActive(true);
        }
    }
}
