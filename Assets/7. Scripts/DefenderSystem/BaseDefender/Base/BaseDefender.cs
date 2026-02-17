using DG.Tweening;
using UnityEngine;

public abstract class BaseDefender : MonoBehaviour, IDefender
{
    [SerializeField] protected DefenderData data;
    public DefenderData Data => data;
    protected int currentHP;
    protected Animator animator;

    protected virtual void Awake()
    {
        if (data == null)
        {
            Debug.LogError("DefenderData missing on " + gameObject.name);
            enabled = false;
            return;
        }
        currentHP = data.maxHP;
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int amount)
    {
        Debug.Log("HP BEFORE: " + currentHP);
        currentHP -= amount;
        Debug.Log("HP AFTER: " + currentHP);
        if (currentHP <= 0)
            Die();
    }

    public virtual void Die()
    {
        if (animator != null)
            animator.SetTrigger(data.deathTrigger);

        var lane = GetComponent<DefenderLane>().Lane;
        DefenderManager.Instance.Unregister(this, lane);

        var grid = PlacementManager.Instance.grid;
        if (grid.TryWorldToCell(transform.position, out Vector2Int cellPos))
        {
            grid.Unoccupy(cellPos);
        }

        Destroy(gameObject, 1f);
    }
}
