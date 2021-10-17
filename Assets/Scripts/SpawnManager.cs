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
    [SerializeField] private GameObject nextTetrominoParent;
    [SerializeField] private GameObject tetrominoesParent;


    #region Monobehaviour

    void Start()
    {
        SpawnNextTetromino();
        SpawnTetromino();
    }

    #endregion

    private void SpawnTetromino()
    {
        DectivateActiveTetromino();
        ActivateNextTetromino();
        SpawnNextTetromino();
    }

    private void DectivateActiveTetromino()
    {
        if(activeTetromino != null)
        {
            lastActiveTetromino = activeTetromino;
            lastActiveTetromino.OnCollision -= SpawnTetromino;
        }
    }

    private void ActivateNextTetromino()
    {
        if (nextTetromino != null)
        {
            activeTetromino = nextTetromino;
            activeTetromino.transform.parent = tetrominoesParent.transform;
            activeTetromino.transform.position = spawnPosition;
            activeTetromino.transform.rotation = Quaternion.Euler(Vector3.zero);
            ChangeLayer(activeTetromino.transform, 8);

            activeTetromino.OnCollision += SpawnTetromino;
            OnTetrominoSpawn?.Invoke(activeTetromino);
        }
    }

    private void SpawnNextTetromino()
    {
        GameObject nextTetromino = GetRandomTetromino();
        GameObject nextTetrominoGameObject = Instantiate<GameObject>(nextTetromino, nextTetrominoParent.transform.position, nextTetromino.transform.rotation, nextTetrominoParent.transform);

        ChangeLayer(nextTetrominoGameObject.transform, 6);
        this.nextTetromino = nextTetrominoGameObject.GetComponent<Tetromino>();
    }
    
    private void ChangeLayer(Transform transform, int layerIndex)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = layerIndex;
        }
    }

    private GameObject GetRandomTetromino()
    {
        int randomTetrominoIndex = Random.Range(0, tetrominoPrefabs.Length - 1);

        return tetrominoPrefabs[randomTetrominoIndex];
    }
}
