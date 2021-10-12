using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public delegate void CollisionAction();
    public event CollisionAction OnCollision;

    public float verticalSpeed = 1f;
    public float horizontalSpeed = 10f;
    public float fastFallModifier = 5f;

    private Rigidbody rigidBody;
    private bool isFalling = true;
    private bool isRotating = false;
    private bool isMovingHorizontaly = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            // Move horizontaly
            //float horizontalInput = Input.GetAxis("Horizontal");
            //transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed * horizontalInput, Space.World);

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
            transform.Translate(Vector3.down * Time.deltaTime * actualFallSpeed, Space.World);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        rigidBody.useGravity = true;
        isFalling = false;
        OnCollision?.Invoke();

        Destroy(this);
    }

    private void Rotate()
    {
        StartCoroutine(RotateOverTime(Vector3.forward, -90.0f, 0.2f));
    }

    private void MoveRight()
    {
        StartCoroutine(MoveOverTime(transform.position + Vector3.right / 2, 0.2f));
    }

    private void MoveLeft()
    {
        StartCoroutine(MoveOverTime(transform.position + Vector3.left / 2, 0.2f));
    }

    private IEnumerator RotateOverTime(Vector3 axis, float angle, float duration)
    {
        isRotating = true;

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
        isRotating = false;
    }

    private IEnumerator MoveOverTime(Vector3 destination, float duration)
    {
        isMovingHorizontaly = true;

        Vector3 origin = transform.position;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(origin, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = destination;
        isMovingHorizontaly = false;
    }
}
