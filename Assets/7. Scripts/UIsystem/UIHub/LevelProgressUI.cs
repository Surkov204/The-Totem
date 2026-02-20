using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour, ILevelUI
{
    [Header("Main Bar")]
    [SerializeField] private Slider slider;

    [Header("Huge Wave Markers")]
    [SerializeField] private RectTransform markerContainer;
    [SerializeField] private GameObject markerPrefab;

    [SerializeField] private LevelRunner runner;

    private float _displayValue;
    private LevelConfig _level;

    // =========================
    // INIT
    // =========================

    private void Awake()
    {
      
    }

    public void InjectRunner(LevelRunner r)
    {
        runner = r;
    }

    public void Init(LevelConfig level)
    {
        _level = level;
    }

    public void OnLevelStart(float totalDuration)
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0f;

        _displayValue = 0f;

        BuildMarkers();
    }

    public void OnLevelTime(float currentTime)
    {
    }

    public void OnLevelEnd()
    {
        slider.value = 1f;
    }

    // =========================
    // GAMEPLAY SYNC
    // =========================

    private void Update()
    {
        if (runner == null) return;

        float target = runner.GetGameplayProgress();

        // Smooth movement
        _displayValue = Mathf.Lerp(_displayValue, target, Time.deltaTime * 6f);

        slider.value = _displayValue;
    }

    // =========================
    // HUGE WAVE MARKERS
    // =========================

    private void BuildMarkers()
    {
        ClearMarkers();

        if (_level == null || _level.waves.Count == 0)
            return;

        float wavePortion = 1f / _level.waves.Count;

        for (int i = 0; i < _level.waves.Count; i++)
        {
            var wave = _level.waves[i];

            if (!wave.isHugeWave) continue;

            float normalized = i * wavePortion;

            var marker = Instantiate(markerPrefab, markerContainer);
            RectTransform rt = marker.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(normalized, 0f);
            rt.anchorMax = new Vector2(normalized, 1f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }

    private void ClearMarkers()
    {
        for (int i = markerContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(markerContainer.GetChild(i).gameObject);
        }
    }

    public void OnHugeWaveStart()
    {
        Debug.Log("Huge Wave!");
    }

    public void OnHugeWaveEnd()
    {
        Debug.Log("Huge Wave End");
    }

    public void ShowHugeWaveApproaching() { }
}
