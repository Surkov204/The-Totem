using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace JS
{
    public enum UIButtonAnimationType
    {
        None,
        SlideLeft,
        SlideRight,
        Fade,
        Scale
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class UIBase : MonoBehaviour
    {
        [Header("Base Config")]
        [SerializeField] private CanvasType canvasType = CanvasType.FullScreen;
        public CanvasType CanvasType => canvasType;

        [Header("Optional: Control Buttons")]
        [SerializeField] private List<Button> listButtonControl = new();

        private CanvasGroup canvasGroup;
        private Tween fadeTween, scaleTween;
        private Tween tween;
        private Vector2 targetPos;
        private Vector2 hiddenPos;

        public System.Type UIType => GetType();

        public bool IsVisible { get; private set; }
        public bool IsAnimating { get; private set; }

        [Header("Button Animation (Optional)")]
        [SerializeField] private bool enableButtonAnimation = false;
        [SerializeField] private UIButtonAnimationType buttonAnimationType = UIButtonAnimationType.None;
        [SerializeField] private float buttonAnimDelay = 1f;
        [SerializeField] private float buttonAnimDuration = 0.5f;
        [SerializeField] private float buttonAnimOffset = 600f;
        [SerializeField] private float buttonAnimInterval = 0.12f;
        [SerializeField] private List<RectTransform> animatedButtons = new();

        [Header("Button Reverse On Hide (Optional)")]
        [SerializeField] private bool reverseButtonAnimationOnHide = false;
        [SerializeField] private float buttonHideDelay = 0f;
        [SerializeField] private float buttonHideDuration = 0.25f;
        [SerializeField] private float buttonHideInterval = 0.06f;
        [SerializeField] private UIAnimationType defaultShowAnimation = UIAnimationType.FadeScale;

        public UIAnimationType LastShowAnimationType { get; private set; } = UIAnimationType.FadeScale;
        // cache target state for each button
        private readonly Dictionary<RectTransform, Vector2> btnTargetPos = new();
        private readonly Dictionary<RectTransform, Vector3> btnTargetScale = new();
        private readonly Dictionary<RectTransform, float> btnTargetAlpha = new();

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);

            OnInit();
        }

        protected virtual void OnInit() { }

        public virtual void OnShow(UIAnimationType type = UIAnimationType.FadeScale)
        {
            if (type == UIAnimationType.FadeScale && defaultShowAnimation != UIAnimationType.FadeScale)
                type = defaultShowAnimation;

            LastShowAnimationType = type;

            switch (type)
            {
                case UIAnimationType.FadeScale: PlayFadeIn(); break;
                case UIAnimationType.SlideLeft: PlaySlideIn(Vector2.left); break;
                case UIAnimationType.SlideRight: PlaySlideIn(Vector2.right); break;
                case UIAnimationType.SlideTop: PlaySlideIn(Vector2.up); break;
                case UIAnimationType.SlideBottom: PlaySlideIn(Vector2.down); break;
            }

            if (enableButtonAnimation)
                PlayButtonAnimation();
        }

        public virtual void OnHide(UIAnimationType type = UIAnimationType.FadeScale)
        {
            if (!enableButtonAnimation || !reverseButtonAnimationOnHide)
            {
                PlayPanelHide(type);
                return;
            }

            PlayButtonReverseAnimation();
            float total = buttonHideDelay + (animatedButtons.Count - 1) * buttonHideInterval + buttonHideDuration;
            DOVirtual.DelayedCall(total, () =>
            {
                PlayPanelHide(type);
            }).SetUpdate(true);
        }

        private RectTransform GetRootCanvasRect()
        {
            var canvas = GetComponentInParent<Canvas>();
            return canvas != null ? canvas.GetComponent<RectTransform>() : null;
        }

        private void PlayPanelHide(UIAnimationType type)
        {
            switch (type)
            {
                case UIAnimationType.FadeScale: PlayFadeOut(); break;
                case UIAnimationType.SlideLeft: PlaySlideOut(Vector2.left); break;
                case UIAnimationType.SlideRight: PlaySlideOut(Vector2.right); break;
                case UIAnimationType.SlideTop: PlaySlideOut(Vector2.up); break;
                case UIAnimationType.SlideBottom: PlaySlideOut(Vector2.down); break;
            }
        }

        public void OnHideAuto()
        {
            OnHide(LastShowAnimationType);
        }

        private void PlayFadeIn()
        {
            if (IsAnimating) return;
            IsAnimating = true;
            IsVisible = true;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            canvasGroup.alpha = 0f;
            transform.localScale = Vector3.one * 0.8f;

            fadeTween?.Kill();
            scaleTween?.Kill();
            fadeTween = canvasGroup.DOFade(1f, 0.2f)
                .SetEase(Ease.InQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    canvasGroup.alpha = 1f;
                    IsAnimating = false;
                });

            scaleTween = transform.DOScale(1f, 0.2f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        private void PlayFadeOut()
        {
            if (IsAnimating || !IsVisible) return;
            IsAnimating = true;
            IsVisible = false;
            fadeTween?.Kill();
            scaleTween?.Kill();

            fadeTween = canvasGroup.DOFade(0f, 0.2f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);

            scaleTween = transform.DOScale(0.9f, 0.2f)
                .SetEase(Ease.InQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    IsAnimating = false;
                });
        }

        private void PlaySlideIn(Vector2 dir)
        {
            if (IsAnimating) return;
            IsAnimating = true; IsVisible = true;
            gameObject.SetActive(true);

            var rect = (RectTransform)transform;

            targetPos = rect.anchoredPosition;

            float offset = (dir.x != 0 ? rect.rect.width : rect.rect.height);
            hiddenPos = targetPos + dir * offset;

            rect.anchoredPosition = hiddenPos;
            canvasGroup.alpha = 1f;

            tween?.Kill();
            tween = rect.DOAnchorPos(targetPos, 0.4f)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    canvasGroup.alpha = 1f;
                    IsAnimating = false;
                });
        }

        private void PlaySlideOut(Vector2 dir)
        {
            if (IsAnimating || !IsVisible) return;
            IsAnimating = true; IsVisible = false;

            var rect = (RectTransform)transform;

            tween?.Kill();
            tween = rect.DOAnchorPos(hiddenPos, 0.3f)
                .SetEase(Ease.InCubic)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    IsAnimating = false;
                });
        }

        public void BlockMultiClick(float delay = 0.2f)
        {
            foreach (var btn in listButtonControl)
                if (btn != null) btn.interactable = false;

            DOVirtual.DelayedCall(delay, () =>
            {
                foreach (var btn in listButtonControl)
                    if (btn != null) btn.interactable = true;
            }).SetUpdate(true);
        }

        public void SetInteractableControlButton(bool value)
        {
            foreach (var button in listButtonControl)
                if (button != null) button.interactable = value;
        }

        private void CacheButtonTargetsIfNeeded(RectTransform btn)
        {
            if (btn == null) return;

            if (!btnTargetPos.ContainsKey(btn))
                btnTargetPos[btn] = btn.anchoredPosition;

            if (!btnTargetScale.ContainsKey(btn))
                btnTargetScale[btn] = btn.localScale;

            if (buttonAnimationType == UIButtonAnimationType.Fade)
            {
                var cg = btn.GetComponent<CanvasGroup>();
                if (cg == null) cg = btn.gameObject.AddComponent<CanvasGroup>();

                if (!btnTargetAlpha.ContainsKey(btn))
                    btnTargetAlpha[btn] = cg.alpha;
            }
        }

        private void KillButtonTweens()
        {
            if (animatedButtons == null) return;

            foreach (var btn in animatedButtons)
            {
                if (btn == null) continue;

                btn.DOKill(true); 
                var cg = btn.GetComponent<CanvasGroup>();
                if (cg != null) cg.DOKill(true);
            }
        }

        private void PlayButtonAnimation()
        {
            if (animatedButtons == null || animatedButtons.Count == 0)
                return;

            KillButtonTweens();

            for (int i = 0; i < animatedButtons.Count; i++)
            {
                var btn = animatedButtons[i];
                if (btn == null) continue;

                CacheButtonTargetsIfNeeded(btn);

                Vector2 tPos = btnTargetPos[btn];
                Vector3 tScale = btnTargetScale[btn];

                Vector2 offscreenPos = GetButtonOffscreenPos(btn, buttonAnimationType);
                float delay = buttonAnimDelay + i * buttonAnimInterval;

                switch (buttonAnimationType)
                {
                    case UIButtonAnimationType.SlideLeft:
                    case UIButtonAnimationType.SlideRight:
                        btn.anchoredPosition = offscreenPos;
                        btn.DOAnchorPos(tPos, buttonAnimDuration)
                            .SetDelay(delay)
                            .SetEase(Ease.OutCubic)
                            .SetUpdate(true);
                        break;

                    case UIButtonAnimationType.Fade:
                        var cg = btn.GetComponent<CanvasGroup>() ?? btn.gameObject.AddComponent<CanvasGroup>();
                        cg.alpha = 0f;
                        cg.DOFade(btnTargetAlpha.TryGetValue(btn, out var a) ? a : 1f, buttonAnimDuration)
                            .SetDelay(delay)
                            .SetUpdate(true);
                        break;

                    case UIButtonAnimationType.Scale:
                        btn.localScale = Vector3.zero;
                        btn.DOScale(tScale, buttonAnimDuration)
                            .SetDelay(delay)
                            .SetEase(Ease.OutBack)
                            .SetUpdate(true);
                        break;
                }
            }
        }

        private Vector2 GetButtonOffscreenPos(RectTransform btn, UIButtonAnimationType type)
        {
            var canvasRect = GetRootCanvasRect();
            Vector2 pos = btnTargetPos[btn];

            if (canvasRect == null)
                return pos;

            float canvasHalfW = canvasRect.rect.width * 0.5f;
            float btnWidth = btn.rect.width;
            float pivotX = btn.pivot.x;

            switch (type)
            {
                case UIButtonAnimationType.SlideLeft:
                    pos.x = -canvasHalfW - btnWidth * (1f - pivotX);
                    break;

                case UIButtonAnimationType.SlideRight:
                    pos.x = canvasHalfW + btnWidth * pivotX;
                    break;
            }

            return pos;
        }

        private void PlayButtonReverseAnimation()
        {
            if (animatedButtons == null || animatedButtons.Count == 0)
                return;

            Sequence seq = DOTween.Sequence().SetUpdate(true);

            for (int i = 0; i < animatedButtons.Count; i++)
            {
                var btn = animatedButtons[i];
                if (btn == null) continue;

                CacheButtonTargetsIfNeeded(btn);
                Vector2 offscreenPos = GetButtonOffscreenPos(btn, buttonAnimationType);

                Tween t = null;

                switch (buttonAnimationType)
                {
                    case UIButtonAnimationType.SlideLeft:
                    case UIButtonAnimationType.SlideRight:
                        t = btn.DOAnchorPos(offscreenPos, buttonHideDuration)
                               .SetEase(Ease.InCubic);
                        break;

                    case UIButtonAnimationType.Fade:
                        var cg = btn.GetComponent<CanvasGroup>() ?? btn.gameObject.AddComponent<CanvasGroup>();
                        t = cg.DOFade(0f, buttonHideDuration);
                        break;

                    case UIButtonAnimationType.Scale:
                        t = btn.DOScale(Vector3.zero, buttonHideDuration)
                               .SetEase(Ease.InQuad);
                        break;
                }

                if (t != null)
                    seq.Insert(buttonHideDelay + i * buttonHideInterval, t);
            }
        }
    }
}
