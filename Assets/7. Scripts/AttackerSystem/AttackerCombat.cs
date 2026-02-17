using UnityEngine;
using System.Collections.Generic;

public class AttackerCombat : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float attackRange = 0.5f;

    private Animator animator;
    private AttackerMovement movement;
    private Attacker attacker;

    private BaseDefender currentTarget;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<AttackerMovement>();
        attacker = GetComponent<Attacker>();
    }

    private void Update()
    {
        if (attacker.Lane < 0) return;

        if (currentTarget == null)
            FindTarget();

        if (currentTarget == null)
        {
            ResumeWalk();
            return;
        }

        float dx = transform.position.x - currentTarget.transform.position.x;

        if (dx <= attackRange)
        {
            StartAttack();
        }
        else
        {
            currentTarget = null;
            ResumeWalk();
        }
    }

    private void FindTarget()
    {
        List<BaseDefender> defenders =
            DefenderManager.Instance.GetLane(attacker.Lane);

        if (defenders == null || defenders.Count == 0) return;

        float myX = transform.position.x;
        float bestDist = float.MaxValue;

        foreach (var defender in defenders)
        {
            if (defender == null) continue;

            float dx = myX - defender.transform.position.x;

            if (dx < 0f) continue;

            if (dx < bestDist)
            {
                bestDist = dx;
                currentTarget = defender;
            }
        }
    }

    private void StartAttack()
    {
        movement.enabled = false;
        animator.SetBool("isWalking", false);

        timer += Time.deltaTime;

        if (timer >= 1f / attackRate)
        {
            timer = 0f;
            animator.SetTrigger("attack");
            currentTarget.TakeDamage(damage);
        }
    }

    private void ResumeWalk()
    {
        movement.enabled = true;
        animator.SetBool("isWalking", true);
    }
}