using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JS;
using EasyTransition;

public class LosePopup : UIBase
{
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
