using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static int LevelToLoad = 0;

    public static void LoadGameplay(int levelIndex)
    {
        LevelToLoad = levelIndex;

        MaskTransitions.TransitionManager.Instance
            .LoadSceneWithTransition("MainGamePlay");
    }

    public static void LoadMenu()
    {
        MaskTransitions.TransitionManager.Instance
            .LoadSceneWithTransition("MainMenu");
    }
}
