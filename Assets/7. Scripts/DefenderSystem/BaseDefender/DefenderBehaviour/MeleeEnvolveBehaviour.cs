using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnvolveBehaviour : MonoBehaviour
{
    [SerializeField] private float evolveDelay = 5f;
    [SerializeField] private float growDuration = 0.5f;

    private MeleeDefender melee;
    private Vector3 baseScale;

    private void Awake()
    {
        melee = GetComponent<MeleeDefender>();
        baseScale = Vector3.one;

        if (melee != null)
            melee.enabled = false; 
    }

    private void Start()
    {
        Invoke(nameof(Evolve), evolveDelay);
    }

    private void Evolve()
    {
        transform.DOScale(baseScale * 1.5f, growDuration)
                 .SetEase(Ease.OutBack)
                 .OnComplete(() =>
                 {
                     if (melee != null)
                         melee.enabled = true;
                 });
    }
}
