using UnityEngine;
using System.Collections;
using Zenject;

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

    [SerializeField] private Transform cardContainerSlot;
    [Inject] private DiContainer _container;
    private GameObject currentLevel;

    public int CurrentLevelIndex { get; private set; }
    public int TotalLevels => levelPrefabs != null ? levelPrefabs.Length : 0;

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
        CurrentLevelIndex = index;

        if (currentLevel != null)
            Destroy(currentLevel);

        currentLevel = _container.InstantiatePrefab(
            levelPrefabs[index],
            levelSpawnPoint
        );

        LevelSettings settings = currentLevel.GetComponent<LevelSettings>();
        if (settings != null)
        {
            settings.Setup(progressUI, sunUI, questUI);
        }

        LevelRunner runner = currentLevel.GetComponent<LevelRunner>();

        if (runner != null)
        {
            runner.SetCardContainer(cardContainerSlot);
        }
    }
}

