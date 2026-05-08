using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EnterNextScene : MonoBehaviour
{
    [Header("Scene à charger")]
    public string sceneToLoad;

    [Header("Position d'apparition")]
    public Vector2 spawnPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerSpawnManager.spawnPosition = spawnPosition;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

