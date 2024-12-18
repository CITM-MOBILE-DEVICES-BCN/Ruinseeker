using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MetaUIManagerRuin : MonoBehaviour
{
    #region Singleton
    public static MetaUIManagerRuin Instance { get; private set; }

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

    [System.Serializable]
    public class LevelUI
    {
        public string levelName;
        public TextMeshProUGUI starsText;
        public Image[] starImages;
        public Button levelButton;
    }

    [Header("Screens")]
    [SerializeField] private GameObject mainScreenCanvas;
    [SerializeField] private GameObject levelSelectScreenCanvas;

    [Header("Global Progress")]
    [SerializeField] private TextMeshProUGUI totalGemsText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Level Progress")]
    [SerializeField] private LevelUI[] levelUIs;

    private void Start()
    {
        UpdateUI();
        ScoreManagerRuin.Instance.OnProgressUpdated += UpdateUI;
    }

    private void OnDestroy()
    {
        if (ScoreManagerRuin.Instance != null)
        {
            ScoreManagerRuin.Instance.OnProgressUpdated -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        // Update total progress
        if (totalGemsText != null)
            totalGemsText.text = $"Total Gems: {ScoreManagerRuin.Instance.TotalGems}";

        if (totalScoreText != null)
            totalScoreText.text = $"Total Score: {ScoreManagerRuin.Instance.TotalScore}";

        // Update each level's progress
        foreach (var levelUI in levelUIs)
        {
            if (levelUI == null) continue;

            var (stars, maxStars) = ScoreManagerRuin.Instance.GetLevelProgress(levelUI.levelName);

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
        SoundManagerRuin.PlaySound(SoundType.BUTTON);
        RuinseekerManager.Instance.ChangeScene(levelName);
        ScoreManagerRuin.Instance.ResetLevelScore();
    }
    public void ShowMainScreen()
    {
       
        if (mainScreenCanvas != null)
            mainScreenCanvas.SetActive(true);
        if (levelSelectScreenCanvas != null)
            levelSelectScreenCanvas.SetActive(false);
    }

    public void ShowLevelSelectScreen()
    {
        SoundManagerRuin.PlaySound(SoundType.BUTTON);
        if (mainScreenCanvas != null)
            mainScreenCanvas.SetActive(false);
        if (levelSelectScreenCanvas != null)
            levelSelectScreenCanvas.SetActive(true);
    }
}