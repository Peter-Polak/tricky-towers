using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public delegate void SpawnAction(Tetromino tetromino);
    public event SpawnAction OnTetrominoSpawn;

    public Tetromino activeTetromino;
    public Tetromino lastActiveTetromino;

    [SerializeField] private GameObject[] tetrominoPrefabs;
    [SerializeField] private Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewTetromino();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void SpawnNewTetromino()
    {
        int randomTetrominoIndex = Random.Range(0, tetrominoPrefabs.Length - 1);
        GameObject newTetrominoGameObject = Instantiate<GameObject>(tetrominoPrefabs[randomTetrominoIndex], spawnPosition, tetrominoPrefabs[randomTetrominoIndex].transform.rotation);

        if(activeTetromino != null) activeTetromino.OnCollision -= SpawnNewTetromino;

        lastActiveTetromino = activeTetromino;
        activeTetromino = newTetrominoGameObject.GetComponent<Tetromino>();

        activeTetromino.OnCollision += SpawnNewTetromino;

        OnTetrominoSpawn?.Invoke(activeTetromino);
    }
}
