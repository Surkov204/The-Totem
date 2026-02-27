using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using JS;
using Zenject;

public class SelectCardPopup : UIBase
{
    [Header("References")]
    [SerializeField] private Button startButton;
    private LevelRunner levelRunner;
    private CardSelectionManager selectionManager;
    private GameSpeedController speedController;

    private IUIService uiService;

    protected override void Awake()
    {
        base.Awake();
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);
    }



    [Inject]
    public void Construct(CardSelectionManager manager, LevelRunner runner, IUIService uIService,GameSpeedController speedController)
    {
        selectionManager = manager;
        levelRunner = runner;
        uiService = uIService;
        this.speedController = speedController;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        speedController.SetInteractable(false);
    }

    private void OnStartClicked()
    {
        List<GameObject> selectedCards =
            selectionManager.GetSelectedCardObjects();

        int selected = selectionManager.GetSelectedCount();
        int required = selectionManager.GetRequiredSlotCount();

        if (selected < required)
            return;

        foreach (var card in selectedCards)
        {
            var placement =
                card.GetComponentInChildren<CardPlacement>();

            if (placement != null)
                placement.SetMode(CardMode.Gameplay);
        }

        Time.timeScale = 1f;
       
        levelRunner.StartLevel();
        speedController.SetInteractable(true);
        uiService.Hide<SelectCardPopup>();
        
    }


}