using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyTransition;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Transition")]
    [SerializeField] private TransitionSettings transitionSetting;

    [SerializeField] private float fillSpeed = 1.5f;
    [SerializeField] private float transitionDelay = 0.25f; 

    private void Start()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        string target = TransitionManager.PendingSceneName;
        if (string.IsNullOrEmpty(target))
            yield break;

        AsyncOperation op = SceneManager.LoadSceneAsync(target);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            if (slider != null)
            {
                slider.value = Mathf.MoveTowards(
                    slider.value,
                    progress,
                    Time.deltaTime * fillSpeed
                );
            }

            yield return null;
        }

        if (slider != null)
            slider.value = 1f;

        yield return new WaitForSeconds(0.15f);

        TransitionManager.Instance().Transition(
            null,              
            transitionSetting,
            0f
        );

        yield return new WaitForSeconds(transitionDelay);

        op.allowSceneActivation = true;
    }
}