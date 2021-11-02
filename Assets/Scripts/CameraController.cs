using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private SpawnManager spawnManager;
    
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
        Debug.Log(startingPosition);
    }

    private void Update()
    {
        if (spawnManager.placedTetrominoes.Count == 0) return;

        Tetromino highestTetromino = spawnManager.placedTetrominoes[0];

        foreach(Tetromino tetromino in spawnManager.placedTetrominoes)
        {
            if (tetromino.gameObject.transform.position.y > highestTetromino.gameObject.transform.position.y)
            {
                highestTetromino = tetromino;
            }
        }

        float difference = Mathf.Abs(highestTetromino.transform.position.y - transform.position.y);

        if(difference > 2)
        {
            Vector3 direction = highestTetromino.transform.position.y > transform.position.y ? Vector3.up : Vector3.down;
            transform.Translate(direction * Time.deltaTime * speed);
        }
    }
}
