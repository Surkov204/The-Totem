using UnityEngine;

public class MeleeDefender : MonoBehaviour
{
    private BaseDefender baseDef;
    private DefenderLane laneInfo;
    private Attacker currentTarget;
    private Animator animator;

    private void Awake()
    {
        baseDef = GetComponent<BaseDefender>();
        laneInfo = GetComponent<DefenderLane>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (laneInfo == null || laneInfo.Lane < 0)
            return;

        if (currentTarget == null)
            FindTarget();

        if (currentTarget == null)
        {
            SetAttackState(false);
            return;
        }

        float dx = currentTarget.transform.position.x - transform.position.x;

        if (dx >= 0f && dx <= baseDef.Data.attackRange)
        {
            SetAttackState(true);
        }
        else
        {
            currentTarget = null;
            SetAttackState(false);
        }
    }

    public void ApplyDamage()
    {
        if (currentTarget != null)
        {
            currentTarget.TakeDamage(baseDef.Data.damage);
        }
    }

    private void SetAttackState(bool attacking)
    {
        if (!string.IsNullOrEmpty(baseDef.Data.attackBool))
            animator.SetBool(baseDef.Data.attackBool, attacking);
    }

    private void FindTarget()
    {
        var attackers = AttackerManager.Instance.GetLane(laneInfo.Lane);
        if (attackers == null || attackers.Count == 0)
            return;

        float myX = transform.position.x;
        float bestDist = float.MaxValue;

        foreach (var attacker in attackers)
        {
            if (attacker == null) continue;

            float dx = attacker.transform.position.x - myX;
            if (dx < 0f) continue;

            if (dx < bestDist)
            {
                bestDist = dx;
                currentTarget = attacker;
            }
        }
    }
}