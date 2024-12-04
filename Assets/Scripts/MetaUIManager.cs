using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MetaUIManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelUI
    {
        public string levelName;
        public TextMeshProUGUI starsText;
        public Image[] starImages;
        public Button levelButton;
    }

    [Header("Global Progress")]
    [SerializeField] private TextMeshProUGUI totalGemsText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Level Progress")]
    [SerializeField] private LevelUI[] levelUIs;

    private void Start()
    {
        UpdateUI();
        ScoreManager.Instance.OnProgressUpdated += UpdateUI;
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnProgressUpdated -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        // Update total progress
        if (totalGemsText != null)
            totalGemsText.text = $"Total Gems: {ScoreManager.Instance.TotalGems}";

        if (totalScoreText != null)
            totalScoreText.text = $"Total Score: {ScoreManager.Instance.TotalScore}";

        // Update each level's progress
        foreach (var levelUI in levelUIs)
        {
            var (stars, maxStars) = ScoreManager.Instance.GetLevelProgress(levelUI.levelName);

            // Update stars text
            if (levelUI.starsText != null)
                levelUI.starsText.text = $"{stars}/{maxStars}";

            // Update star images if they exist
            if (levelUI.starImages != null)
            {
                for (int i = 0; i < levelUI.starImages.Length; i++)
                {
                    if (levelUI.starImages[i] != null)
                        levelUI.starImages[i].enabled = i < stars;
                }
            }
        }
    }

    // Can be called from button OnClick events
    public void LoadLevel(string levelName)
    {
        GameManager.Instance.ChangeScene(levelName);
    }
}