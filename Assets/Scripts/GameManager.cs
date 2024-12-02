using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using NavigationSystem;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private NavigationManager navManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion

    Vector2 checkpointPos;

    private void Start()
    {
        navManager = new NavigationManager();
        checkpointPos = transform.position;
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
    }

    public void ChangeScene(string sceneName)
    {
        navManager.ChangeScene(sceneName);
        ScoreManager.Instance.ResetLevelScore();
    }

    public void ActivateCanvas(GameObject canvas)
    {
        navManager.ActivateCanvas(canvas);
    }

    public void DeactivateCanvas(GameObject canvas)
    {
        navManager.DeactivateCanvas(canvas);
    }

    public void FinishLevel(ScoreManager scoreManager)
    {
        scoreManager.FinishLevel();
    }
}
