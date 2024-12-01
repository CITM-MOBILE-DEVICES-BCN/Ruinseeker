using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatatulaEnemy : Enemy
{
    private bool isAttached = false;

    public override void Patrol() { }

    public override void OnPlayerDetected()
    {
        if (!isAttached)
        {
            AttachToPlayer();
        }
    }

    private void AttachToPlayer()
    {
        isAttached = true;
        transform.SetParent(player);
        transform.localPosition = Vector3.zero;
        StartCoroutine(InvertControls());    
    }

    private IEnumerator InvertControls()
    {
        // Cambiar behaviour maybe?
        // PlayerController.Instance.InvertControls(true);
        yield return new WaitForSeconds(5f);
        // Falta invertir los controles, o el playerMovement es un Singleton o pillo una referencia al player
        Die();
    }

    public override void Die()
    {
        transform.SetParent(null);
        base.Die();
    }
}
