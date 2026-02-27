using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static int LevelToLoad = 0;

    public const string SCENE_LOADING = "LoadingScene";
    public const string SCENE_MAIN_MENU = "MainHome";
    public const string SCENE_SELECT_MAP = "MainSelectMap";
    public const string SCENE_GAMEPLAY = "GamePlayMain";

    public static void LoadSceneViaLoading(
        string targetScene,
        TransitionSettings settings,
        float startDelay = 0f
    )
    {
        TransitionManager.SetPendingScene(targetScene);
        TransitionManager.Instance().Transition(SCENE_LOADING, settings, startDelay);
    }

    public static void LoadMenu(TransitionSettings settings, float startDelay = 0f)
        => LoadSceneViaLoading(SCENE_MAIN_MENU, settings, startDelay);

    public static void LoadSelectMap(TransitionSettings settings, float startDelay = 0f)
        => LoadSceneViaLoading(SCENE_SELECT_MAP, settings, startDelay);

    public static void LoadGameplay(int levelIndex, TransitionSettings settings, float startDelay = 0f)
    {
        LevelToLoad = levelIndex;
        LoadSceneViaLoading(SCENE_GAMEPLAY, settings, startDelay);
    }

    public static void ReloadCurrent(TransitionSettings settings, float startDelay = 0f)
    {
        string current = SceneManager.GetActiveScene().name;
        LoadSceneViaLoading(current, settings, startDelay);
    }

    public static void RetryGameplay(TransitionSettings settings, float startDelay = 0f)
        => LoadSceneViaLoading(SCENE_GAMEPLAY, settings, startDelay);

    public static void RetryGameplay(int levelIndex, TransitionSettings settings, float startDelay = 0f)
    {
        LevelToLoad = levelIndex;
        LoadSceneViaLoading(SCENE_GAMEPLAY, settings, startDelay);
    }
}