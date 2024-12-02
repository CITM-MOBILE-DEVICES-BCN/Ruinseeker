using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
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
    [SerializeField] private int gemsPerStar = 5;
    [SerializeField] private int pointsPerStar = 10000;

    private int currentGems;
    private int totalScore;
    private int gemsAtLastCheckpoint;

    public int CurrentGems => currentGems;
    public int TotalScore => totalScore;

    public void AddGems(int amount)
    {
        currentGems += amount;
        // Could trigger UI update event here
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

    public void FinishLevel()
    {
        int stars = CalculateStars();
        totalScore += stars * pointsPerStar;

        // Here you could save the score/stars to persistent storage
        SaveLevelProgress(stars);
    }

    private void SaveLevelProgress(int stars)
    {
        // Implementation for saving progress (could use PlayerPrefs or your own save system)
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt($"{currentSceneName}_Stars", stars);
        PlayerPrefs.SetInt($"{currentSceneName}_Score", totalScore);
        PlayerPrefs.Save();
    }

    // Call this when loading a new level
    public void ResetLevelScore()
    {
        currentGems = 0;
        gemsAtLastCheckpoint = 0;
    }
}
