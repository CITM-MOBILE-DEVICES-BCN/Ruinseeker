using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEnemy : Enemy
{
    [SerializeField] private float movementDelay = 1.0f;
    private Queue<Vector3> playerPositions = new Queue<Vector3>();
    private bool isRecording = false;

    protected override void Start()
    {
        base.Start();
        if (player == null)
        {
            Debug.LogError("Player transform not assigned to shadow");
        }
    }

    public override void Patrol()
    {
        if (playerPositions.Count > 0)
        {
            Vector3 nextPosition = playerPositions.Dequeue();
            Debug.Log($"Mimicking position: {nextPosition}");
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        }
    }

    public override void OnPlayerDetected()
    {
        if (!isRecording)
        {
            StartCoroutine(RecordPlayerMovement());
            isRecording = true;
        }
    }

    private IEnumerator RecordPlayerMovement()
    {
        if (player != null)
        {
            playerPositions.Enqueue(player.position);
            Debug.Log($"Recording position: {player.position}");

            if (playerPositions.Count > Mathf.CeilToInt(movementDelay / Time.fixedDeltaTime))
            {
                playerPositions.Dequeue();
            }
        }
        else
        {
            Debug.LogWarning("Player transform is null when assigning movements");
        }

        yield return new WaitForFixedUpdate();
    }
}
