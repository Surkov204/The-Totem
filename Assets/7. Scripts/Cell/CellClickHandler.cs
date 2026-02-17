using UnityEngine;
using UnityEngine.EventSystems;

public class CellClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlacementManager.Instance == null)
            return;

        PlacementManager.Instance.TryPlaceAtScreenPos(eventData.position);
    }
}