using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    #region Events

    public delegate void SpawnAction(Tetromino tetromino);
    public event SpawnAction OnTetrominoSpawn;

    #endregion

    public List<Tetromino> placedTetrominoes;
    public Tetromino activeTetromino;
    public Tetromino nextTetromino;

    [SerializeField] private GameObject milestonePrefab;
    [SerializeField] private GameObject[] tetrominoPrefabs;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject nextTetrominoParent;
    [SerializeField] private GameObject tetrominoesParent;


    #region Monobehaviour

    void Start()
    {
        SpawnNextTetromino();
        SpawnTetromino();
        SpawnMilestone(new Vector3(0, 5, 0));

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

    private void SpawnMilestone(Vector3 spawnPosition)
    {
        GameObject milestoneGameObject = Instantiate(milestonePrefab, spawnPosition, Quaternion.Euler(0, 0, 0));
        Milestone milestone = milestoneGameObject.GetComponent<Milestone>();
        milestone.OnTrigger += (Tetromino tetromino) => { FreezePlacedTeromionoes(); };
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
            activeTetromino = null;
        }
    }

    private void ActivateNextTetromino()
    {
        if (nextTetromino != null)
        {
            Vector3 newSpawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, Camera.main.transform.position.z * -1));
            Debug.Log(newSpawnPosition);

            activeTetromino = nextTetromino;
            activeTetromino.transform.parent = tetrominoesParent.transform;
            activeTetromino.transform.position = newSpawnPosition;
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

    private void FreezePlacedTeromionoes(int quantityFromEnd)
    {
        for (int index = placedTetrominoes.Count - quantityFromEnd; index < placedTetrominoes.Count; index++)
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
