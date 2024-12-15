using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManagerRuin.PlaySound(SoundType.CHECKPOINT);
            RuinseekerManager.Instance.UpdateCheckpointPosition(transform.position);
            Debug.Log("checkpoint saved");
        }

    }
}
