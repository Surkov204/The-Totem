using JS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PopupListDefender : UIBase
{
    [Header("Buttons")]
    [SerializeField] private Button exitButton;

    private IUIService uiService;

    [Inject]
    public void Construct(IUIService uiService)
    {
        this.uiService = uiService;

    }

    protected override void Awake()
    {
        base.Awake();

        if (exitButton != null)
            exitButton.onClick.AddListener(exitButtonPress);
    }

    private void exitButtonPress() {
        uiService.Hide<PopupListDefender>();
    }
}
