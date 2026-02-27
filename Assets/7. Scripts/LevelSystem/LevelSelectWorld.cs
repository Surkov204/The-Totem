using UnityEngine;
using EasyTransition;

public class LevelSelectWorld : MonoBehaviour
{
    [SerializeField] private int levelIndex;

    [Header("Transition")]
    [SerializeField] private TransitionSettings transitionSetting;
    [SerializeField] private float startDelay = 0f;

    [Header("Ruby child")]
    [SerializeField] private GameObject ruby; 

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        bool unlocked = LevelProgression.IsUnlocked(levelIndex);
        if (ruby != null) ruby.SetActive(unlocked);
    }

    public void Select()
    {
        if (!LevelProgression.IsUnlocked(levelIndex))
            return;

        SceneLoader.LoadGameplay(levelIndex, transitionSetting, startDelay);
    }
}