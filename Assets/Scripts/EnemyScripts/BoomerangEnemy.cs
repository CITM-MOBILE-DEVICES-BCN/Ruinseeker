using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangEnemy : Enemy
{
    public GameObject boomerangPrefab;
    public float boomerangSpeed = 5f;
    public float boomerangRange = 10f;

    private bool isThrowingBoomerang;

    public override void Patrol()
    {
        // Lógica de patrulla (No hay)s
    }

    public override void OnPlayerDetected()
    {
        if (!isThrowingBoomerang)
        {
            StartCoroutine(ThrowBoomerang());
        }
    }

    private IEnumerator ThrowBoomerang()
    {
        isThrowingBoomerang = true;
        var boomerang = Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
        var boomerangScript = boomerang.GetComponent<Boomerang>();
        boomerangScript.Initialize(transform.position, boomerangRange, boomerangSpeed);
        yield return new WaitUntil(() => boomerangScript.HasReturned);
        isThrowingBoomerang = false;
    }

    public override void Die()
    {
        Debug.Log("Boomerang Guy defeated!");
        base.Die();
    }
}
