using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [Header("Visualization")]
    [SerializeField] private bool showTriggerArea = true;
    [SerializeField] private Color gizmoColor = new Color(0, 1, 0, 0.3f);

    [Header("Trigger Settings")]
    [SerializeField] private TriggerResponse[] triggerResponses;

    private BoxCollider2D triggerArea;

    private void Awake()
    {
        triggerArea = GetComponent<BoxCollider2D>();
        if (triggerArea == null)
        {
            triggerArea = gameObject.AddComponent<BoxCollider2D>();
            triggerArea.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var response in triggerResponses)
        {
            if (other.CompareTag(response.targetTag))
            {
                // Regular Unity Events
                response.onTriggerEnter?.Invoke();

                // Singleton Events
                if (GameManager.Instance != null)
                    response.gameManagerEvents?.Invoke(GameManager.Instance);

                if (ScoreManager.Instance != null)
                    response.scoreManagerEvents?.Invoke(ScoreManager.Instance);

                if (ScoreUIManager.Instance != null)
                    response.uiManagerEvents?.Invoke(ScoreUIManager.Instance);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (var response in triggerResponses)
        {
            if (other.CompareTag(response.targetTag))
            {
                response.onTriggerExit?.Invoke();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        foreach (var response in triggerResponses)
        {
            if (other.CompareTag(response.targetTag))
            {
                response.onTriggerStay?.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showTriggerArea)
        {
            Gizmos.color = gizmoColor;
            if (triggerArea != null)
            {
                Vector3 center = transform.position + new Vector3(triggerArea.offset.x, triggerArea.offset.y, 0);
                Vector3 size = new Vector3(triggerArea.size.x, triggerArea.size.y, 0.1f);
                Gizmos.DrawCube(center, size);
            }
        }
    }
}