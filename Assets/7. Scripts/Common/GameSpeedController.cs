using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedLabel;
    [SerializeField] private Button speedButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private float[] speeds = { 1f, 1.5f };
    private int currentIndex = 0;

    private void Start()
    {
        ApplySpeed();
    }

    public void ToggleSpeed()
    {
        if (!speedButton.interactable)
            return;

        currentIndex++;

        if (currentIndex >= speeds.Length)
            currentIndex = 0;

        ApplySpeed();
    }

    private void ApplySpeed()
    {
        Time.timeScale = speeds[currentIndex];
        speedLabel.text = "x" + Time.timeScale.ToString("0.#");
    }

    public void SetInteractable(bool value)
    {
        speedButton.interactable = value;

        if (canvasGroup != null)
            canvasGroup.alpha = value ? 1f : 0.4f;
    }
}