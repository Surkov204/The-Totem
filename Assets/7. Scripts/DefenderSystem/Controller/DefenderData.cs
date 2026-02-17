using UnityEngine;

[CreateAssetMenu(menuName = "Game/Defender Data")]
public class DefenderData : ScriptableObject
{
    [Header("Economy")]
    public int cost;
    public float placementCooldown;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Stats")]
    public int maxHP;
    public float attackRange;
    public int damage;
    public float attackRate;

    [Header("Animation")]
    public string attackTrigger;
    public string deathTrigger;
    public string attackBool;
}
