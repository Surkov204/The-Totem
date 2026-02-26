using UnityEngine;
using EasyTransition;
using JS;
using Zenject;
using js;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private IUIService uiService;

    [Inject]
    public void Construct(IUIService uiService)
    {
        this.uiService = uiService;
    }

    private void Start()
    {
        uiService.Show<MainMenuPopup>();
    }
}
