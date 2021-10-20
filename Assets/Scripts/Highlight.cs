using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    private Tetromino tetromino;

    #region Monobehaviour

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
        if(tetromino != null)
        {
            transform.localScale = new Vector3(tetromino.CurrentWidth / 10, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(tetromino.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    #endregion

    private void OnTetrominoSpawnHandler(Tetromino tetromino)
    {
        this.tetromino = tetromino;
    }
}
