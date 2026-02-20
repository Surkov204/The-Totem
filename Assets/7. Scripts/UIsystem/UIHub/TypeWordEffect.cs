using TMPro;
using UnityEngine;
using System.Collections;

public class TypeWordEffect : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private float wordDelay = 0.3f;

    private Coroutine runningCoroutine;

    // =====================================
    // PLAY + WAIT (dùng cho Runner yield)
    // =====================================
    public IEnumerator PlayAndWait(string fullText)
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        yield return runningCoroutine = StartCoroutine(TypeEffect(fullText));
    }

    // =====================================
    // PLAY ONLY (optional)
    // =====================================
    public void Play(string fullText)
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(TypeEffect(fullText));
    }

    // =====================================
    // CORE TYPE LOGIC
    // =====================================
    private IEnumerator TypeEffect(string text)
    {
        targetText.text = "";

        string[] words = text.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            yield return new WaitForSecondsRealtime(wordDelay);

            targetText.text += words[i];

            if (i < words.Length - 1)
                targetText.text += " ";
        }

        runningCoroutine = null;
    }
}
