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
        }
        else if (IsPlayerInRange())
        {
            OnPlayerDetected();  
        }
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
