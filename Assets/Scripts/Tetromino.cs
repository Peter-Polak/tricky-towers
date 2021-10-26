using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    #region Events

    public delegate void CollisionAction();
    public event CollisionAction OnCollision;

    public delegate void DespawnAction();
    public event DespawnAction OnDespawn;

    #endregion

    public TetrominoStatus Status { private set; get; } = TetrominoStatus.INACTIVE;
    [HideInInspector] public bool IsRotating { private set; get; } = false;
    [HideInInspector] public bool IsMovingHorizontaly { private set; get; } = false;

    public float width = 2;
    public float height = 3;
    public Material fallingMaterial;
    public Material placedMaterial;

    public float CurrentWidth { private set; get; }

    private Rigidbody rigidBody;

    #region Monobehaviour

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        CurrentWidth = width;
        SetMaterial(fallingMaterial);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Place();
        OnCollision?.Invoke();
    }

    #endregion

    #region Tetromino controll and movement methods

    public void RotateRight()
    {
        StartCoroutine(RotateOverTime(Vector3.forward, -90.0f, 0.2f));
        if (CurrentWidth == width) CurrentWidth = height; else CurrentWidth = width;
    }

    public void RotateLeft()
    {
        StartCoroutine(RotateOverTime(Vector3.forward, 90.0f, 0.2f));
        if (CurrentWidth == width) CurrentWidth = height; else CurrentWidth = width;
    }

    public void MoveRight()
    {
        StartCoroutine(MoveOverTime(transform.position + Vector3.right / 2, 0.2f));
    }

    public void MoveLeft()
    {
        StartCoroutine(MoveOverTime(transform.position + Vector3.left / 2, 0.2f));
    }

    public void Freeze()
    {
        //rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        rigidBody.isKinematic = true;
    }

    // Code from: http://answers.unity.com/answers/1236502/view.html Thanks!
    private IEnumerator RotateOverTime(Vector3 axis, float angle, float duration)
    {
        IsRotating = true;

        Quaternion originalRotation = transform.rotation;
        Quaternion newRotation = originalRotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, newRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = newRotation;
        IsRotating = false;
    }

    private IEnumerator MoveOverTime(Vector3 destination, float duration)
    {
        IsMovingHorizontaly = true;

        Vector3 origin = transform.position;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(origin, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = destination;
        IsMovingHorizontaly = false;
    }

    #endregion

    private void SetMaterial(Material material)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = material;
        }
    }

    public void StartFalling()
    {
        Status = TetrominoStatus.FALLING;
    }

    public void Place()
    {
        Status = TetrominoStatus.PLACED;
        rigidBody.mass = 100;
        rigidBody.useGravity = true;
        rigidBody.velocity = Vector3.zero;

        SetMaterial(placedMaterial);
    }
}

public enum TetrominoStatus
{
    INACTIVE,
    FALLING,
    PLACED
}