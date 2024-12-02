using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEnemy : Enemy
{
    private bool isActivated = false;
    private Vector3 targetPosition;

    public override void Patrol()
    {
        if (isActivated)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Die();
            }
        }
        else if (IsPlayerInRange())
        {
            OnPlayerDetected();
        }

    }

    protected override void Update()
    {
        Patrol();
    }

    public override void OnPlayerDetected()
    {
        isActivated = true;
        targetPosition = player.position;
    }

    public override void Die()
    {
        Debug.Log("Submarine Mine destroyed!");
        base.Die();
    }
}
