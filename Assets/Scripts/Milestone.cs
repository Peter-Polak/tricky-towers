using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestone : MonoBehaviour
{
    public delegate void OnTriggerEnterAction(Tetromino tetromino);
    public event OnTriggerEnterAction OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        Tetromino tetromino = other.GetComponent<Tetromino>();

        if (tetromino == null) return;

        if (tetromino.Status == TetrominoStatus.PLACED)
        {
            OnTrigger?.Invoke(tetromino);
            Debug.Log("Milestone reached!", tetromino);
        }
    }
}
