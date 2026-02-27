using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSelectionManager : MonoBehaviour
{
    [SerializeField] private List<CardSlotUI> slots;
    private List<GameObject> selectedCards = new();
    private Stack<(CardSlotUI slot, GameObject popupCard)> selectionOrder = new();

    public void SelectCard(GameObject popupCard)
    {
        CardSlotUI slot = GetFirstEmptySlot();
        if (slot == null) return;

        var placement = popupCard.GetComponent<CardPlacement>();
        if (placement == null) return;

        GameObject newCard = slot.SpawnCard(popupCard);
        if (newCard == null) return;

        newCard.GetComponent<CardPlacement>().SetMode(CardMode.InSlot);

        selectedCards.Add(newCard);

        selectionOrder.Push((slot, popupCard));

        SetPopupCardDisabled(popupCard);
    }

    public void RemoveLastSelected()
    {
        if (selectionOrder.Count == 0)
            return;

        var (slot, popupCard) = selectionOrder.Pop();

        if (!slot.IsEmpty)
        {
            selectedCards.Remove(slot.transform.GetChild(0).gameObject);
        }

        slot.ClearSlot();
        SetPopupCardEnabled(popupCard);
    }

    private void SetPopupCardDisabled(GameObject popupCard)
    {
        CanvasGroup cg = popupCard.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = popupCard.AddComponent<CanvasGroup>();

        cg.alpha = 0.4f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    private void SetPopupCardEnabled(GameObject popupCard)
    {
        CanvasGroup cg = popupCard.GetComponent<CanvasGroup>();
        if (cg == null) return;

        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;

    }

    private CardSlotUI GetFirstEmptySlot()
    {
        foreach (var slot in slots)
            if (slot.IsEmpty)
                return slot;

        return null;
    }

    public List<GameObject> GetSelectedCardObjects()
    {
        return selectedCards;
    }

    public int GetSelectedCount()
    {
        return selectedCards.Count;
    }

    public int GetRequiredSlotCount()
    {
        return slots.Count;
    }
}