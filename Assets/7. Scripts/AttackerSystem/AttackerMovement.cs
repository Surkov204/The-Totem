using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AttackerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;

    private Animator animator;
    private Attacker attacker;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attacker = GetComponent<Attacker>();
    }

    private void Start()
    {
        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    private void Update()
    {
        if (attacker == null) return;
        if (attacker.IsDead) return;

        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}