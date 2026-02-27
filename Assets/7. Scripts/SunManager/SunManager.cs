using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SunManager : MonoBehaviour
{
    public static SunManager Instance;

    [SerializeField] private int currentSun = 100;
    [SerializeField] private Transform sunIconWorldTarget;
    private TextMeshProUGUI sunText;

    private void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void InjectUI(TextMeshProUGUI text)
    {
        sunText = text;
        Debug.Log(text.text);
        UpdateUI();
    }

    public int CurrentSun => currentSun;

    public bool CanAfford(int cost)
    {
        return currentSun >= cost;
    }

    public void Spend(int amount)
    {
        currentSun -= amount;
        if (currentSun < 0)
            currentSun = 0;

        UpdateUI();
    }

    public void Add(int amount)
    {
        currentSun += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (sunText != null)
            sunText.text = currentSun.ToString();
    }

    public Transform GetSunTarget()
    {
        return sunIconWorldTarget;
    }
}
