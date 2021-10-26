using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public delegate void SpawnAction(Tetromino tetromino);
    public event SpawnAction OnTetrominoSpawn;

    public List<Tetromino> placedTetrominoes;
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

        if(IsSpawnPointFree())
        {
            ActivateNextTetromino();
            SpawnNextTetromino();
        }
    }

    private bool IsSpawnPointFree()
    {
        Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, 3);

        return hitColliders.Length > 0 ? false : true;
    }

    private void DectivateActiveTetromino()
    {
        if(activeTetromino != null)
        {
            activeTetromino.OnCollision -= SpawnTetromino;
            placedTetrominoes.Add(activeTetromino);
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

            activeTetromino.StartFalling();
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

    private void FreezePlacedTeromionoes(int startIndexInc, int quantity)
    {
        for(int index = startIndexInc; index < startIndexInc + quantity; index++)
        {
            placedTetrominoes[index].Freeze();
        }
    }

    private void FreezePlacedTeromionoes()
    {
        placedTetrominoes.ForEach(
            (Tetromino tetromino) =>
            {
                tetromino.Freeze();
            }
        );
    }
}
