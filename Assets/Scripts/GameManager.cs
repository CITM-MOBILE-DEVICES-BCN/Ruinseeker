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
    private SaveSystem saveSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        saveSystem = new SaveSystem();
    }
    #endregion

    Vector2 checkpointPosition;

    private void Start()
    {
        navManager = new NavigationManager();
        SaveSystem saveSystem = new SaveSystem();
        SaveData saveData = saveSystem.Load();
        checkpointPosition = saveData.lastCheckpointPosition;
        GameObject.FindGameObjectsWithTag("Player")[0].transform.position = checkpointPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            saveSystem.DeleteSave();
        }
    }

    public void UpdateCheckpointPosition(Vector2 pos)
    {
        checkpointPosition = pos;

        SaveData saveData = new SaveData()
        {
            lastCheckpointPosition = checkpointPosition
        };

        saveSystem.Save(saveData);
    }

    public Vector2 GetCheckpointPosition()
    { 
        return checkpointPosition; 
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
