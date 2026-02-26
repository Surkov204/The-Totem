using TMPro;
using UnityEngine;

public class UnlockedLevelCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text rubyText;
    [SerializeField] private int totalLevels = 60;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (rubyText == null) return;
        rubyText.text = LevelProgression.GetUnlockedCount(totalLevels).ToString();
    }
}