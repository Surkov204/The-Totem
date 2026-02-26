using UnityEngine;

public static class LevelProgression
{
    private const string KEY = "MAX_UNLOCKED_LEVEL";
    private const int DEFAULT_MAX = 0; 

    public static int MaxUnlocked => PlayerPrefs.GetInt(KEY, DEFAULT_MAX);

    public static bool IsUnlocked(int levelIndex) => levelIndex <= MaxUnlocked;

    public static int GetUnlockedCount(int totalLevels)
    {
        if (totalLevels <= 0) return 0;
        return Mathf.Clamp(MaxUnlocked + 1, 1, totalLevels);
    }

    public static void OnWin(int levelIndex, int totalLevels)
    {
        if (levelIndex == MaxUnlocked)
        {
            int next = Mathf.Clamp(MaxUnlocked + 1, 0, totalLevels - 1);
            PlayerPrefs.SetInt(KEY, next);
            PlayerPrefs.Save();
        }
    }
}