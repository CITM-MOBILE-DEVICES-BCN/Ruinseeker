using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    protected Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public abstract void Patrol();

    protected virtual void Update()
    {
        if (IsPlayerInRange())
        {
            OnPlayerDetected();
        }
        else
        {
            Patrol();
        }
    }

    public abstract void OnPlayerDetected();

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    protected bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

}


