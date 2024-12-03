using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public event Action<int> OnGemsChanged;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnStarsChanged;

    #region Singleton
    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("Score Settings")]
    [SerializeField] private int gemsPerStar = 2;
    [SerializeField] private int pointsPerStar = 10000;

    private int currentGems;
    private int totalScore;
    private int gemsAtLastCheckpoint;

    public int CurrentGems => currentGems;
    public int TotalScore => totalScore;
    public bool IsLevelCompleted => hasLevelBeenCompleted;


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FinishLine"))
        {

        }
    }

    public void AddGems(int amount)
    {
        currentGems += amount;
        Debug.Log("Current Gems: " + currentGems);
        OnGemsChanged?.Invoke(currentGems);
    }

    public bool TrySpendGems(int amount)
    {
        if (currentGems >= amount)
        {
            currentGems -= amount;
            return true;
        }
        return false;
    }

    public void SaveCheckpoint()
    {
        gemsAtLastCheckpoint = currentGems;
    }

    public void ResetToLastCheckpoint()
    {
        currentGems = gemsAtLastCheckpoint;
    }

    public int CalculateStars()
    {
        return Mathf.Min(3, currentGems / gemsPerStar);

    }

    private bool hasLevelBeenCompleted = false;
    public void FinishLevel()
    {
        if (hasLevelBeenCompleted) return;

        int stars = CalculateStars();
        totalScore += stars * pointsPerStar;

        OnScoreChanged?.Invoke(totalScore);
        OnStarsChanged?.Invoke(stars);

        hasLevelBeenCompleted = true;

        SaveLevelProgress(stars);

        Debug.Log("Level has finished");
        Debug.Log($"Stars: {stars}");
        Debug.Log($"Score: {totalScore}");
        Debug.Log($"Gems: {currentGems}");
    }

    public void SaveLevelProgress(int stars)
    {
        // Implementation for saving progress (could use PlayerPrefs or your own save system)
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt($"{currentSceneName}_Stars", stars);
        PlayerPrefs.SetInt($"{currentSceneName}_Score", totalScore);
        PlayerPrefs.Save();
    }

    public void ResetLevelScore()
    {
        currentGems = 0;
        gemsAtLastCheckpoint = 0;
        hasLevelBeenCompleted = false;
    }
}
