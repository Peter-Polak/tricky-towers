using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public delegate void SpawnAction(Tetromino tetromino);
    public event SpawnAction OnTetrominoSpawn;

    public Tetromino lastActiveTetromino;
    public Tetromino activeTetromino;
    public Tetromino nextTetromino;

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
        GameObject newTetromino = GetRandomTetromino();
        GameObject nextTetromino = GetRandomTetromino();
        GameObject newTetrominoGameObject = Instantiate<GameObject>(newTetromino, spawnPosition, newTetromino.transform.rotation);

        lastActiveTetromino = activeTetromino;
        activeTetromino = newTetrominoGameObject.GetComponent<Tetromino>();

        activeTetromino.OnCollision += SpawnNewTetromino;
        if(lastActiveTetromino != null) lastActiveTetromino.OnCollision -= SpawnNewTetromino;

        OnTetrominoSpawn?.Invoke(activeTetromino);
    }

    private GameObject GetRandomTetromino()
    {
        int randomTetrominoIndex = Random.Range(0, tetrominoPrefabs.Length - 1);

        return tetrominoPrefabs[randomTetrominoIndex];
    }
}
