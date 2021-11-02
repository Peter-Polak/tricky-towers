using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float distanceDifferenceY = 1.0f;
    [SerializeField] private float offsetY = 5.0f;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private SpawnManager spawnManager;
    
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (spawnManager.placedTetrominoes.Count == 0) return;

        Tetromino highestTetromino = GetHighestTetromino();
        MoveCamera(highestTetromino);
    }

    private Tetromino GetHighestTetromino()
    {
        Tetromino highestTetromino = spawnManager.placedTetrominoes[0];

        foreach (Tetromino tetromino in spawnManager.placedTetrominoes)
        {
            if (tetromino.gameObject.transform.position.y > highestTetromino.gameObject.transform.position.y)
            {
                highestTetromino = tetromino;
            }
        }

        return highestTetromino;
    }

    private void MoveCamera(Tetromino highestTetromino)
    {
        float difference = Mathf.Abs(highestTetromino.transform.position.y + offsetY - transform.position.y);

        if (difference > distanceDifferenceY)
        {
            Vector3 direction = highestTetromino.transform.position.y + offsetY > transform.position.y ? Vector3.up : Vector3.down;
            transform.Translate(direction * Time.deltaTime * speed);
        }
    }
}
