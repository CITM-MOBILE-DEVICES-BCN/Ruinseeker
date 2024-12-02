using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.UpdateCheckpoint(transform.position);

        }

    }
}
