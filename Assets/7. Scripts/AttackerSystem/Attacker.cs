using UnityEngine;
using DG.Tweening;

public class Attacker : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHP = 100;

    public int Lane { get; private set; } = -1;

    private Tween hitTween;
    private Tween flashTween;
    private int currentHP;
    private Animator animator;
    private bool isDead;
    public bool IsDead => isDead;

    private Renderer[] renderers;
    private MaterialPropertyBlock mpb;

    private static readonly int BaseColorID =
        Shader.PropertyToID("_BaseColor");

    private static readonly int EmissionID =
        Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();

        renderers = GetComponentsInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void SetLane(int lane)
    {
        Lane = lane;

        if (AttackerManager.Instance != null)
            AttackerManager.Instance.Register(this);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;

        PlayHitFlash();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void PlayHitFlash()
    {
        if (flashTween != null && flashTween.IsActive())
            flashTween.Kill();

        if (hitTween != null && hitTween.IsActive())
            hitTween.Kill();

        transform.localScale = Vector3.one;

        foreach (var r in renderers)
        {
            if (!r || !r.sharedMaterial) continue;

            r.GetPropertyBlock(mpb);

            if (r.sharedMaterial.HasProperty(BaseColorID))
                mpb.SetColor(BaseColorID, Color.red);

            if (r.sharedMaterial.HasProperty(EmissionID))
                mpb.SetColor(EmissionID, Color.white * 3f);

            r.SetPropertyBlock(mpb);
        }

        flashTween = DOVirtual.DelayedCall(0.12f, () =>
        {
            foreach (var r in renderers)
            {
                if (!r) continue;
                r.SetPropertyBlock(null);
            }
        });

        hitTween = transform.DOPunchScale(
            Vector3.one * 0.15f,
            0.15f,
            6,
            0.5f
        );
    }

    private void Die()
    {
        isDead = true;

        if (AttackerManager.Instance != null)
            AttackerManager.Instance.Unregister(this);

        if (animator != null)
            animator.SetTrigger("die");

        Destroy(gameObject, 1.5f);
    }

    private void OnDisable()
    {
        if (AttackerManager.Instance != null)
            AttackerManager.Instance.Unregister(this);
    }
}