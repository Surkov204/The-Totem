using UnityEngine;
using System.Collections;
using TMPro;

public class QuestTitleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private TypeWordEffect typeWordEffect;

    [Header("Position")]
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 targetAnchoredPosition;

    [Header("Size")]
    [SerializeField] private float startSize = 150f;
    [SerializeField] private float endSize = 50f;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 1.5f;
    [SerializeField] private float holdTime = 1.2f;

    private void Awake()
    {
        questText.gameObject.SetActive(false);
    }

    // ===============================
    // INTRO ONLY (NO SHRINK)
    // ===============================
    public IEnumerator PlayIntroOnly(string text)
    {
        questText.gameObject.SetActive(true);
        questText.fontSize = startSize;
        questText.rectTransform.anchoredPosition = startPos;

        yield return typeWordEffect.PlayAndWait(text);

        yield return new WaitForSecondsRealtime(0.4f);
    }

    // ===============================
    // WAVE FULL SEQUENCE
    // ===============================
    public IEnumerator PlayWaveSequence(string text)
    {
        questText.gameObject.SetActive(true);
        questText.fontSize = startSize;
        questText.rectTransform.anchoredPosition = startPos;

        yield return typeWordEffect.PlayAndWait(text);

        yield return new WaitForSecondsRealtime(0.3f);

        float timer = 0f;
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveDuration);

            questText.rectTransform.anchoredPosition =
                Vector2.Lerp(startPos, targetAnchoredPosition, t);

            questText.fontSize =
                Mathf.Lerp(startSize, endSize, t);

            yield return null;
        }

        yield return new WaitForSecondsRealtime(holdTime);
    }
}
