using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BottomPanelToggle : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform btnUp;
    [SerializeField] private Button btnDownButton;
    [SerializeField] private Button btnUpButton;

    [SerializeField] private float duration = 0.4f;

    private float panelHeight;
    private bool isOpen = true;

    private void Start()
    {
        panelHeight = panel.rect.height;

        btnDownButton.onClick.AddListener(Hide);
        btnUpButton.onClick.AddListener(Show);
    }

    private void Hide()
    {
        if (!isOpen) return;
        isOpen = false;

        panel.DOAnchorPosY(-panelHeight, duration).SetEase(Ease.InOutCubic);

        btnUp.DOAnchorPosY(100, duration).SetEase(Ease.InOutCubic);
    }

    private void Show()
    {
        if (isOpen) return;
        isOpen = true;

        panel.DOAnchorPosY(0, duration).SetEase(Ease.InOutCubic);

        btnUp.DOAnchorPosY(-100, duration).SetEase(Ease.InOutCubic);
    }
}