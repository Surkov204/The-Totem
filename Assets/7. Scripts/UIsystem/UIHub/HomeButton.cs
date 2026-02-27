using EasyTransition;
using js;
using JS;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HomeButton : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private Button OnHomeButton;
    [SerializeField] private Button OnListButton;

    private IUIService uiService;
    private const string MAIN_GAMEPLAY_SCENE = "MainHome";

    [Inject]
    public void Construct(IUIService uiService)
    {
        this.uiService = uiService;
    }

    private void Awake()
    {
        OnHomeButton?.onClick.AddListener(OnHomePress);
        OnListButton?.onClick.AddListener(OnListPress);
    }

    public void OnHomePress()
    {
        TransitionManager.SetPendingScene(MAIN_GAMEPLAY_SCENE);

        TransitionManager.Instance().Transition(
            "LoadingScene",
            transitionSettings,
            startDelay
        );
    }

    public void OnListPress()
    {
        uiService.Show<PopupListDefender>();
    }
}
