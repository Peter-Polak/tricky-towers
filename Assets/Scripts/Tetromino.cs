using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    #region Events

    public delegate void CollisionAction();
    public event CollisionAction OnCollision;

    public delegate void DespawnAction();
    public event CollisionAction OnDespawn;

    #endregion

    [SerializeField] private float despawnY = -5;

    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position.y < despawnY)
        {
            OnDespawn?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rigidBody.useGravity = true;
        rigidBody.velocity = Vector3.zero;
        OnCollision?.Invoke();

        Destroy(this);
    }
}
