using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackable
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool alwaysAttack = false;

    private DefenderData data;
    private Animator animator;
    private DefenderLane laneInfo;
    private float timer;
    private Transform currentTarget;

    private void Awake()
    {
        BaseDefender defender = GetComponent<BaseDefender>();
        data = defender.Data;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        laneInfo = GetComponent<DefenderLane>();

        if (data.attackRate <= 0f)
            return;

        if (!alwaysAttack)
        {
            FindTarget();

            if (currentTarget == null)
            {
                timer = 0f;
                return;
            }
        }

        timer += Time.deltaTime;

        if (timer >= 1f / data.attackRate)
        {
            timer = 0f;
            Attack();
        }
    }

    public void Attack()
    {
        if (animator == null || string.IsNullOrEmpty(data.attackTrigger))
            return;

        animator.SetTrigger(data.attackTrigger);
    }

    public void SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("PROJECTILE PREFAB IS NULL");
            return;
        }

        GameObject projectile = Instantiate(
            projectilePrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Projectile p = projectile.GetComponent<Projectile>();

        if (p != null)
        {
            if (alwaysAttack)
            {
                p.SetTarget(null, data.damage);
            }
            else if (currentTarget != null)
            {
                p.SetTarget(currentTarget, data.damage);
            }
        }
    }

    private void FindTarget()
    {
        currentTarget = null;

        if (laneInfo == null) return;
        if (laneInfo.Lane < 0) return;

        var list = AttackerManager.Instance.GetLane(laneInfo.Lane);
        if (list == null || list.Count == 0) return;

        float myX = transform.position.x;
        float bestDist = float.MaxValue;

        foreach (var attacker in list)
        {
            if (attacker == null) continue;

            float dx = attacker.transform.position.x - myX;

            if (dx <= 0f) continue;

            if (dx > data.attackRange) continue;

            if (dx < bestDist)
            {
                bestDist = dx;
                currentTarget = attacker.transform;
            }
        }
    }
}