using JS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class LootingPopup : UIBase
{
    [Header("Buttons")]
    [SerializeField] private Button continueButton;
    private IUIService uiService;

    [Inject]
    public void Construct(IUIService uiService)
    {
        this.uiService = uiService;
    }

    protected override void Awake()
    {
        base.Awake();

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinue);
    }

    private void OnContinue()
    {
        uiService.Hide<LootingPopup>();
    }
}
