using JS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private Button PauseButton;

    private IUIService uiService;

    [Inject]
    public void Construct(IUIService uiService)
    {
        this.uiService = uiService;
    }

    private void Start()
    {
        StartCoroutine(ShowSelectPopupDelay());
    }

    private IEnumerator ShowSelectPopupDelay()
    {
        yield return new WaitForSeconds(1f);
        uiService.Show<SelectCardPopup>();
    }

    private void Awake()
    {
        if (PauseButton != null)
            PauseButton.onClick.AddListener(OnPauseClick);
    }

    public void OnPauseClick()
    {
        Debug.Log("Click");
        uiService.Show<PausePopup>();
        Time.timeScale = 0;
    }
}
