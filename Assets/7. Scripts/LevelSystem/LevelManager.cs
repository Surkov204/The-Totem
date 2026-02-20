using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private Transform levelSpawnPoint;

    [Header("Test Mode If you want to load level fast")]
    [SerializeField] private bool isTestMode;
    [SerializeField] private int LevelTestIndex;

    [Header("Scene UI")]
    [SerializeField] private LevelProgressUI progressUI;
    [SerializeField] private SunUIReference sunUI;
    [SerializeField] private QuestTitleUI questUI;

    private GameObject currentLevel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!isTestMode)
            LoadLevel(SceneLoader.LevelToLoad);
        else
            LoadLevel(LevelTestIndex);
    }

    public void LoadLevel(int index)
    {
        if (currentLevel != null)
            Destroy(currentLevel);

        currentLevel = Instantiate(levelPrefabs[index], levelSpawnPoint);

        LevelSettings settings = currentLevel.GetComponent<LevelSettings>();
        if (settings != null)
        {
            settings.Setup(progressUI, sunUI, questUI);
        }
    }
}

