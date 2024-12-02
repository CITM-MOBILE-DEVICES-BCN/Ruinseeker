using UnityEngine;
using UnityEngine.Events;
using System;

// Custom serializable UnityEvents for each singleton type
[Serializable]
public class GameManagerEvent : UnityEvent<GameManager> { }

[Serializable]
public class ScoreManagerEvent : UnityEvent<ScoreManager> { }

[Serializable]
public class UIManagerEvent : UnityEvent<ScoreUIManager> { }

[Serializable]
public class TriggerResponse
{
    [Tooltip("Tag to check for collision")]
    public string targetTag = "Player";

    [Header("Regular Events")]
    [Tooltip("Called when object enters trigger")]
    public UnityEvent onTriggerEnter = new UnityEvent();
    [Tooltip("Called when object exits trigger")]
    public UnityEvent onTriggerExit = new UnityEvent();
    [Tooltip("Called while object stays in trigger")]
    public UnityEvent onTriggerStay = new UnityEvent();

    [Header("Singleton Events")]
    [Tooltip("Game Manager methods to call on enter")]
    public GameManagerEvent gameManagerEvents = new GameManagerEvent();
    [Tooltip("Score Manager methods to call on enter")]
    public ScoreManagerEvent scoreManagerEvents = new ScoreManagerEvent();
    [Tooltip("UI Manager methods to call on enter")]
    public UIManagerEvent uiManagerEvents = new UIManagerEvent();
}
