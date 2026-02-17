using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnScaleAnimation : MonoBehaviour
{
    [SerializeField] private float duaration = 0.35f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private Vector3 originScale;

    private void Awake()
    {
        originScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        transform.DOScale(originScale, duaration).SetEase(ease);
    }
}
