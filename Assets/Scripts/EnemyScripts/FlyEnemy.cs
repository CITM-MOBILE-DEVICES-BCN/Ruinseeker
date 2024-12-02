using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{
    private bool isChasingPlayer = false;
    
    public override void Patrol()
    {
        // Lógica de patrulla 
    }

    public override void OnPlayerDetected()
    {
        isChasingPlayer = true;
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    public override void Die()
    {
        Debug.Log("Fly defeated!");
        base.Die();
    }
}
