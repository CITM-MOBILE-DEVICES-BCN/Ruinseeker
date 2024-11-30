using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEnemy : Enemy
{
    private Vector3[] playerPositions;
    private int currentIndex = 0;
    private float recordDelay = 0.5f;

    protected override void Start()
    {
        base.Start();
        
    }

    public override void Patrol()
    {
        if (playerPositions.Length > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPositions[currentIndex], moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerPositions[currentIndex]) < 0.1f)
            {
                currentIndex = (currentIndex + 1) % playerPositions.Length;
            }
        }
    }

    public override void OnPlayerDetected()
    {
        
    }

    private IEnumerator RecordPlayerMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(recordDelay);
            if (playerPositions == null)
            {
                playerPositions = new Vector3[20];
            }

            playerPositions[currentIndex] = player.position;
            currentIndex = (currentIndex + 1) % playerPositions.Length;
        }
    }
}
