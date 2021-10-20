using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    private Tetromino tetromino;

    private void OnEnable()
    {
        spawnManager.OnTetrominoSpawn += OnTetrominoSpawnHandler;
    }
    private void OnDisable()
    {
        spawnManager.OnTetrominoSpawn -= OnTetrominoSpawnHandler;
    }

    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, tetromino.width);
        transform.position = new Vector3(tetromino.transform.position.x, transform.position.y, transform.position.z);
    }

    private void OnTetrominoSpawnHandler(Tetromino tetromino)
    {
        this.tetromino = tetromino;
    }
}
