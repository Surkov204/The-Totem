using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string levelName;

    [Header("Map")]
    public GameObject mapPrefab;

    [Header("Gameplay")]
    public LevelConfig levelConfig;

    [Header("Meta")]
    public LevelType levelType;
    public bool isFinalLevel;
}
