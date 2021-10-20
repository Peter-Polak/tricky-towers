using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    [SerializeField] private float verticalSpeed = 1f;
    [SerializeField] private float fastFallModifier = 5f;
    [SerializeField] private SpawnManager spawnManager;

    private Tetromino tetromino;
    private Transform tetrominoTransform;

    #region Monobehaviour

    private void OnEnable()
    {
        spawnManager.OnTetrominoSpawn += SetControlledTetromino;
    }

    private void OnDisable()
    {
        spawnManager.OnTetrominoSpawn -= SetControlledTetromino;
    }

    private void Update()
    {
        if(tetromino != null)
        {
            if (!tetromino.IsMovingHorizontaly)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    tetromino.MoveLeft();
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    tetromino.MoveRight();
                }
            }

            // Rotate
            if (!tetromino.IsRotating)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    tetromino.RotateRight();
                }
            }

            // Fast fall?
            float actualFallSpeed = verticalSpeed;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                actualFallSpeed *= fastFallModifier;
            }

            // Fall down
            tetrominoTransform.Translate(Vector3.down * Time.deltaTime * actualFallSpeed, Space.World);
        }
    }

    #endregion

    #region Event callbacks

    private void SetControlledTetromino(Tetromino tetromino)
    {
        this.tetromino = tetromino;
        tetrominoTransform = tetromino.gameObject.transform;
    }

    #endregion
}