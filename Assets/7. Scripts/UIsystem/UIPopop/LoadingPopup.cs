using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using EasyTransition;

namespace JS
{
    public class LoadingPopup : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        [Header("Transition when entering target scene")]
        [SerializeField] private TransitionSettings enterTransition;

        private void Start()
        {
            StartCoroutine(LoadTargetScene());
        }

        private IEnumerator LoadTargetScene()
        {
            string target = GetPendingScene();

            if (string.IsNullOrEmpty(target))
            {
                Debug.LogError("Pending scene is empty.");
                yield break;
            }

            slider.value = 0f;

            var op = SceneManager.LoadSceneAsync(target);
            op.allowSceneActivation = false;


            while (op.progress < 0.9f)
            {
                slider.value = op.progress / 0.9f;
                yield return null;
            }

            slider.value = 1f;

            yield return new WaitForSecondsRealtime(0.15f);

            op.allowSceneActivation = true;

            while (!op.isDone)
                yield return null;

            yield return null; 
            TransitionManager.Instance().Transition(
                enterTransition,
                0f
            );
        }

        private string GetPendingScene()
        {
            return null;
        }
    }
}