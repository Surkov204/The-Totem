using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
public class UICuteIconAnim : MonoBehaviour
{
    [Header("Scale (breathing)")]
    [Tooltip("Scale max when pop")]
    public float popScale = 1.12f;
    [Tooltip("Time to scale up")]
    public float popUpDuration = 0.22f;
    [Tooltip("Time to return scale")]
    public float popDownDuration = 0.26f;

    [Header("Wiggle (rotation)")]
    [Tooltip("Rotation wiggle angle in degrees")]
    public float wiggleAngle = 8f;
    [Tooltip("How long one wiggle sequence lasts")]
    public float wiggleDuration = 0.9f;
    [Tooltip("How many wiggles inside the sequence")]
    public int wiggleVibrato = 10;
    [Tooltip("Randomness of wiggle")]
    public float wiggleRandomness = 40f;

    [Header("Optional float (position)")]
    public bool enableFloat = true;
    public float floatDistance = 10f;
    public float floatDuration = 1.2f;

    [Header("Loop")]
    public float loopGapMin = 0.4f;
    public float loopGapMax = 1.2f;
    [Tooltip("Random start delay so icons don't move together")]
    public float startDelayMax = 0.6f;

    [Header("Misc")]
    public bool unscaledTime = true; // UI often wants ignore Time.timeScale

    Vector3 baseScale;
    Vector3 basePos;
    Quaternion baseRot;

    Sequence seq;

    void Awake()
    {
        var rt = (RectTransform)transform;
        baseScale = rt.localScale;
        basePos = rt.anchoredPosition3D;
        baseRot = rt.localRotation;
    }

    void OnEnable()
    {
        Play();
    }

    void OnDisable()
    {
        Stop(true);
    }

    public void Play()
    {
        Stop(false);

        float startDelay = Random.Range(0f, startDelayMax);

        seq = DOTween.Sequence();
        if (unscaledTime) seq.SetUpdate(true);
        seq.SetDelay(startDelay);

        // Loop forever with a nice cute rhythm
        seq.AppendCallback(() =>
        {
            // ensure starting from base
            transform.DOKill();
            var rt = (RectTransform)transform;
            rt.localScale = baseScale;
            rt.localRotation = baseRot;
            rt.anchoredPosition3D = basePos;
        });

        // POP
        seq.Append(transform.DOScale(baseScale * popScale, popUpDuration).SetEase(Ease.OutBack));

        // WIGGLE during pop-down (looks cute)
         seq.Join(
             transform.DOShakeRotation(
                 wiggleDuration,                       // e.g. 0.35f ~ 0.55f
                 new Vector3(0, 0, wiggleAngle),       // e.g. 12f ~ 20f
                 wiggleVibrato,                        // e.g. 18 ~ 30 (cang cao cang rung nhanh)
                 wiggleRandomness,                     // e.g. 60 ~ 120
                 fadeOut: true
             ).OnComplete(() => transform.localRotation = baseRot)
         );

        // FLOAT (optional)
        if (enableFloat)
        {
            var rt = (RectTransform)transform;
            seq.Join(rt.DOAnchorPos3D(basePos + new Vector3(0, floatDistance, 0), floatDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo));
        }

        // RETURN
        seq.Append(transform.DOScale(baseScale, popDownDuration).SetEase(Ease.OutCubic));

        // Random gap then loop
        seq.AppendInterval(Random.Range(loopGapMin, loopGapMax));

        seq.SetLoops(-1, LoopType.Restart);
    }

    public void Stop(bool snapToBase)
    {
        if (seq != null && seq.IsActive())
            seq.Kill();

        transform.DOKill();

        if (snapToBase)
        {
            var rt = (RectTransform)transform;
            rt.localScale = baseScale;
            rt.localRotation = baseRot;
            rt.anchoredPosition3D = basePos;
        }
    }
}
