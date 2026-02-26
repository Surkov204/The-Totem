using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JS;
using EasyTransition;

public class WinnerPopup : UIBase
{
    private IUIService uiService;

    [SerializeField] private Button ExitButton;
    [SerializeField] TransitionSettings transitionSettings;
    [SerializeField] private float delaydelay = 0f;

    protected override void Awake()
    {
        base.Awake();

        if (ExitButton != null)
            ExitButton.onClick.AddListener(OnExitPress);
    }

    private void OnExitPress() {
        SceneLoader.LoadSelectMap(transitionSettings, delaydelay);
    }
}
