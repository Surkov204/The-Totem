using JS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelRunner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager grid;
     private LevelConfig level;

    [Header("Optional hooks")]
    [SerializeField] private MonoBehaviour specialEventReceiver;
    [SerializeField] private MonoBehaviour levelUI;


    private ISpecialEventReceiver _special;
    private ILevelUI _ui;
    private IUIService uiService;

    private float _timer;
    private int _waveIndex;
    private int _specialIndex;

    private bool _running;
    private bool _waveActive;
    private bool _waveSpawningDone;

    private QuestTitleUI _questUI;
    private bool _isPlayingTitle;

    private List<DefenderCardData> selectedCards;
    private Transform cardContainer;

    private void Awake()
    {
        _special = specialEventReceiver as ISpecialEventReceiver;
        _ui = levelUI as ILevelUI;
    }

    [Inject]
    public void Construct(IUIService uiService)
    {
        this.uiService = uiService;
    }

    public void SetCardContainer(Transform container)
    {
        cardContainer = container;
    }

    public void InjectSelectedPrefabs(List<GameObject> prefabs)
    {
        if (cardContainer == null)
        {
            Debug.LogError("CardContainer not inject.");
            return;
        }

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (var prefab in prefabs)
        {
            Instantiate(prefab, cardContainer);
        }
    }

    public void InjectQuestUI(QuestTitleUI questUI)
    {
        _questUI = questUI;
    }

    public void StartLevel()
    {
        if (grid == null || level == null)
        {
            Debug.LogError("Missing grid or level config.");
            enabled = false;
            return;
        }

        level.waves.Sort((a, b) => a.startTime.CompareTo(b.startTime));
        level.specialEvents.Sort((a, b) => a.triggerTime.CompareTo(b.triggerTime));

        _timer = 0f;
        _waveIndex = 0;
        _specialIndex = 0;
        _running = false; 
        _waveActive = false;
        _waveSpawningDone = false;

        StartCoroutine(StartLevelFlow());
    }
    private void Update()
    {
        if (!_running) return;

        _timer += Time.deltaTime;
        _ui?.OnLevelTime(_timer);

        HandleSpecialEvents();
        HandleWaveLogic();

        if (CheckWin())
        {
            _running = false;
            LevelProgression.OnWin(LevelManager.Instance.CurrentLevelIndex,
                       LevelManager.Instance.TotalLevels);

            uiService.Show<WinnerPopup>();
            _ui?.OnLevelEnd();
        }
    }

    private bool CheckWin()
    {
        return _waveIndex >= level.waves.Count &&
               !_waveActive &&
               AttackerManager.Instance.AliveCount() == 0;
    }

    private IEnumerator StartLevelFlow()
    {
        if (!string.IsNullOrEmpty(level.introText) && _questUI != null)
        {
            yield return _questUI.PlayIntroOnly(level.introText);
        }

        _running = true;
        _ui?.OnLevelStart(level.totalDuration);
    }

    // =========================
    // WAVE CONTROL
    // =========================

    private void HandleWaveLogic()
    {
        if (_waveIndex >= level.waves.Count)
            return;

        var wave = level.waves[_waveIndex];

        if (!_waveActive)
        {
            if (CanStartWave(wave))
            {
                StartCoroutine(RunWave(wave));
                _waveActive = true;
            }

            return;
        }

        if (_waveActive && _waveSpawningDone)
        {
            if (AttackerManager.Instance.AliveCount() == 0)
            {
                _waveActive = false;
                _waveSpawningDone = false;
                _waveIndex++;
            }
        }
    }

    private bool CanStartWave(WaveTimeline wave)
    {
        switch (wave.triggerMode)
        {
            case WaveTriggerMode.TimeOnly:
                return _timer >= wave.startTime;

            case WaveTriggerMode.WhenPreviousCleared:
                return AttackerManager.Instance.AliveCount() == 0;

            case WaveTriggerMode.TimeOrCleared:
                return _timer >= wave.startTime ||
                       AttackerManager.Instance.AliveCount() == 0;
        }

        return false;
    }

    // =========================
    // RUN WAVE
    // =========================

    private IEnumerator RunWave(WaveTimeline wave)
    {
        _waveSpawningDone = false;

        if (!string.IsNullOrEmpty(wave.waveTitle) && _questUI != null)
        {
            yield return _questUI.PlayWaveSequence(wave.waveTitle);
        }

        float waveStart = Time.time;

        if (wave.isHugeWave)
            _ui?.OnHugeWaveStart();

        foreach (var se in wave.spawnEvents)
        {
            bool spawned = false;

            while (!spawned)
            {
                float elapsed = Time.time - waveStart;
                int alive = AttackerManager.Instance.AliveCount();

                bool timeReached = elapsed >= se.delayFromWaveStart;
                bool clearReached = false;

                switch (se.gateMode)
                {
                    case SpawnGateMode.WaitUntilClear:
                        clearReached = alive == 0;
                        break;

                    case SpawnGateMode.WaitUntilRemaining:
                        clearReached = alive <= se.remainingThreshold;
                        break;

                    case SpawnGateMode.WaitOrTimeout:
                        clearReached =
                            alive <= se.remainingThreshold ||
                            elapsed >= se.delayFromWaveStart + se.timeout;
                        break;
                }

                if (timeReached || clearReached)
                {
                    yield return StartCoroutine(ExecuteSpawn(se));
                    spawned = true;
                }
                else
                {
                    yield return null;
                }
            }
        }

        if (wave.isHugeWave)
            _ui?.OnHugeWaveEnd();

        _waveSpawningDone = true;
    }

    // =========================
    // EXECUTE SPAWN (MULTI ENTRY)
    // =========================

    private IEnumerator ExecuteSpawn(SpawnEvent se)
    {
        if (se.entries == null || se.entries.Count == 0)
            yield break;

        foreach (var entry in se.entries)
        {
            if (entry.prefab == null)
                continue;

            for (int i = 0; i < entry.count; i++)
            {
                int lane = ResolveLane(entry.laneMode, entry.fixedLane);
                lane = Mathf.Clamp(lane, 0, grid.rows - 1);

                int spawnCol = entry.overrideSpawnColumn
                    ? Mathf.Clamp(entry.spawnColumn, 0, grid.cols - 1)
                    : grid.cols - 1;

                Vector2Int cell = new Vector2Int(spawnCol, lane);
                Vector3 pos = grid.CellToWorldCenter(cell);

                var go = Instantiate(entry.prefab, pos, Quaternion.Euler(0f, -90f, 0f));

                var attacker = go.GetComponent<Attacker>();
                if (attacker != null)
                    attacker.SetLane(lane);

                if (entry.interval > 0f)
                    yield return new WaitForSeconds(entry.interval);
                else
                    yield return null;
            }
        }
    }

    private int ResolveLane(SpawnLaneMode mode, int fixedLane)
    {
        switch (mode)
        {
            case SpawnLaneMode.Fixed:
                return fixedLane;

            case SpawnLaneMode.Random:
                return Random.Range(0, grid.rows);

            case SpawnLaneMode.WeakestLane:
                return PickWeakestLane();
        }

        return 0;
    }

    private int PickWeakestLane()
    {
        int bestLane = 0;
        int min = int.MaxValue;

        for (int r = 0; r < grid.rows; r++)
        {
            int count = 0;
            for (int c = 0; c < grid.cols; c++)
            {
                if (grid.GetAt(new Vector2Int(c, r)) != null)
                    count++;
            }

            if (count < min)
            {
                min = count;
                bestLane = r;
            }
        }

        return bestLane;
    }

    // =========================
    // SPECIAL EVENTS
    // =========================

    private void HandleSpecialEvents()
    {
        while (_specialIndex < level.specialEvents.Count &&
               _timer >= level.specialEvents[_specialIndex].triggerTime)
        {
            _special?.OnSpecialEvent(level.specialEvents[_specialIndex]);
            _specialIndex++;
        }
    }

    public float GetGameplayProgress()
    {
        if (level == null || level.waves.Count == 0)
            return 0f;

        float wavePortion = 1f / level.waves.Count;

        float baseProgress = _waveIndex * wavePortion;

        float waveInternal = 0f;

        if (_waveActive && !_waveSpawningDone)
            waveInternal = 0.5f; 

        if (_waveActive && _waveSpawningDone)
            waveInternal = 0.8f; 

        return Mathf.Clamp01(baseProgress + waveInternal * wavePortion);
    }

    public void SetLevelConfig(LevelConfig config)
    {
        level = config;
    }
}
