using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    Normal,
    Endless
}

public class LevelSettings : MonoBehaviour
{
    [Header("Config")]
    public LevelType levelType;
    public bool isFinalLevel;

    [Header("References")]
    [SerializeField] private LevelRunner runner;
    [SerializeField] private LevelConfig config;
    [SerializeField] private SunManager sunManager;

    public void Setup(LevelProgressUI ui, SunUIReference sunUI, QuestTitleUI questUI)
    {
        if (runner == null || config == null)
        {
            Debug.LogError("Runner or Config missing.");
            return;
        }

        runner.SetLevelConfig(config);

        if (ui != null)
        {
            ui.InjectRunner(runner);
            ui.Init(config);
        }

        if (questUI != null)
            runner.InjectQuestUI(questUI);

        if (sunManager != null && sunUI != null)
        {
            sunManager.InjectUI(sunUI.SunText);
        }

        runner.StartLevel();
    }
}

