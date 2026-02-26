using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class CardSlotUI : MonoBehaviour
{
    private GameObject currentCard;
    [SerializeField] private CardSelectionManager manager;

    public bool IsEmpty => currentCard == null;
    [Inject] private DiContainer container;

    public GameObject SpawnCard(GameObject cardPrefab)
    {
        if (!IsEmpty) return null;

        currentCard = container.InstantiatePrefab(cardPrefab, transform);
        var placement = currentCard.GetComponent<CardPlacement>();
        placement.SetInSlot(true);
        placement.SetParentSlot(this);
        RectTransform rect = currentCard.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        var img = currentCard.GetComponent<Image>();
        if (img != null)
            img.raycastTarget = false;

        return currentCard;
    }

    public void ClearSlot()
    {
        if (currentCard != null)
        {
            Destroy(currentCard);
            currentCard = null;
        }
    }
}