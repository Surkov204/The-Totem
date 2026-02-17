using UnityEngine;
using UnityEngine.UI;

public class CardCooldown : MonoBehaviour
{
     private float cooldownDuration = 5f;
    [SerializeField] private Image cooldownImage;

    private float timer;
    private bool cooling;

    private void Update()
    {
        if (!cooling) return;

        timer -= Time.deltaTime;

        cooldownImage.fillAmount = timer / cooldownDuration;

        if (timer <= 0f)
        {
            cooling = false;
            cooldownImage.fillAmount = 0f;
        }
    }

    public void Init(float duration)
    {
        cooldownDuration = duration;
    }

    public bool IsReady()
    {
        return !cooling;
    }

    public void TriggerCooldown()
    {
        cooling = true;
        timer = cooldownDuration;
        cooldownImage.fillAmount = 1f;
    }
}
