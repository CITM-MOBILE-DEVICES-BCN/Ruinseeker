using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{
    private bool isChasingPlayer = false;
    
    public override void Patrol()
    {
        if (!isChasingPlayer && IsPlayerInRange())
        {
            OnPlayerDetected();
        }

        if (isChasingPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    public override void OnPlayerDetected()
    {
        isChasingPlayer = true;
    }

    public override void Die()
    {
        Debug.Log("Fly defeated!");
        base.Die();
    }
}
