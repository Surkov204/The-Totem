using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using System;

public enum CanvasType
{
    FullScreen,
    HUD,
    Popup,
    Loading
}

namespace JS
{
    public class UIManager : MonoBehaviour, IUIService
    {
        [SerializeField] private UIConfigExtend uiConfig;
        [SerializeField] private Transform uiRoot;
        [Inject] private DiContainer container;
        private UIBase currentFullScreen;
        private UIBase currentHUD;

        private Stack<UIBase> popupStack = new();
        private Dictionary<Type, UIBase> spawned = new();

        public bool HasPopupOnTop => popupStack.Count > 0;

        private void Start()
        {
            foreach (var pair in uiConfig.GetAll())
            {
                if (!pair.preload || pair.prefab == null) continue;

                var instance = Instantiate(pair.prefab, uiRoot);
                instance.gameObject.SetActive(false);
                spawned[pair.prefab.GetType()] = instance;
            }
        }

        public void Show<T>() where T : UIBase
        {
            var type = typeof(T);

            var ui = GetOrSpawn(type, out var pair);
            if (ui == null) return;

            switch (pair.canvasType)
            {
                case CanvasType.FullScreen:
                    currentFullScreen?.OnHide(pair.animationType);
                    currentFullScreen = ui;
                    break;

                case CanvasType.HUD:
                    currentHUD?.OnHide(pair.animationType);
                    currentHUD = ui;
                    break;

                case CanvasType.Popup:
                    popupStack.Push(ui);
                    break;
            }

            ui.OnShow(pair.animationType);
        }

        public void Hide<T>() where T : UIBase
        {
            var type = typeof(T);
            if (!spawned.TryGetValue(type, out var ui)) return;

            var pair = uiConfig.Get(type);
            if (pair == null) return;

            if (pair.canvasType == CanvasType.Popup && popupStack.Count > 0 && popupStack.Peek() == ui)
                popupStack.Pop();

            ui.OnHide(pair.animationType);

            if (pair.destroyAfterHide)
            {
                DOVirtual.DelayedCall(0.35f, () =>
                {
                    Destroy(ui.gameObject);
                    spawned.Remove(type);
                }, true);
            }
        }

        public bool IsVisible<T>() where T : UIBase
        {
            return spawned.TryGetValue(typeof(T), out var ui) && ui.IsVisible;
        }

        public void Back()
        {
            if (popupStack.Count == 0) return;

            var top = popupStack.Pop();
            var pair = uiConfig.Get(top.UIType);
            var type = pair != null ? pair.animationType : UIAnimationType.FadeScale;
            top.OnHide(type);
        }

        public void HideAll()
        {
            currentFullScreen?.OnHide();
            currentHUD?.OnHide();

            while (popupStack.Count > 0)
                popupStack.Pop().OnHide();

            currentFullScreen = null;
            currentHUD = null;
        }

        private UIBase GetOrSpawn(Type type, out UIConfigExtend.UIPairExtend pair)
        {
            pair = uiConfig.Get(type);
            if (pair == null)
            {
                Debug.LogError($"[UIManager] No UIConfig for {type.Name}");
                return null;
            }

            if (spawned.TryGetValue(type, out var ui) && ui != null)
                return ui;

            ui = container.InstantiatePrefabForComponent<UIBase>
            (
                pair.prefab,
                uiRoot
            );

            spawned[type] = ui;
            return ui;
        }
    }
}
