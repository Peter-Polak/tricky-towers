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

    private bool isRotating = false;
    private bool isMovingHorizontaly = false;

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
            if (!isMovingHorizontaly)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    MoveLeft();
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    MoveRight();
                }
            }

            // Rotate
            if (!isRotating)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Rotate();
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

    #region Event callbacks

    private void SetControlledTetromino(Tetromino tetromino)
    {
        this.tetromino = tetromino;
        tetrominoTransform = tetromino.gameObject.transform;
    }

    #endregion

    #region Tetromino controll and movement methods

    private void Rotate()
    {
        StartCoroutine(RotateOverTime(Vector3.forward, -90.0f, 0.2f));
    }

    private void MoveRight()
    {
        StartCoroutine(MoveOverTime(tetrominoTransform.position + Vector3.right / 2, 0.2f));
    }

    private void MoveLeft()
    {
        StartCoroutine(MoveOverTime(tetrominoTransform.position + Vector3.left / 2, 0.2f));
    }

    // Code from: http://answers.unity.com/answers/1236502/view.html Thanks!
    private IEnumerator RotateOverTime(Vector3 axis, float angle, float duration)
    {
        isRotating = true;

        Quaternion originalRotation = tetrominoTransform.rotation;
        Quaternion newRotation = originalRotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            tetrominoTransform.rotation = Quaternion.Slerp(originalRotation, newRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tetrominoTransform.rotation = newRotation;
        isRotating = false;
    }

    private IEnumerator MoveOverTime(Vector3 destination, float duration)
    {
        isMovingHorizontaly = true;

        Vector3 origin = tetrominoTransform.position;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            tetrominoTransform.localPosition = Vector3.Lerp(origin, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tetrominoTransform.localPosition = destination;
        isMovingHorizontaly = false;
    }

    #endregion
}