using UnityEngine;
using JS;
using Zenject;

public class LoseZone : MonoBehaviour
{
    private IUIService uiService;
    private bool triggered;

    [Inject]
    public void Construct(IUIService uiService)
    {
        this.uiService = uiService;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        Attacker attacker = other.GetComponent<Attacker>();
        if (attacker == null) return;

        triggered = true;

        Debug.Log("LOSE");

        Time.timeScale = 0f;
        uiService.Show<LosePopup>();
    }
}