using UnityEngine;

public class CardSelectableItemUI : MonoBehaviour
{
    private CardSelectionManager manager;

    public void Init(CardSelectionManager mng)
    {
        manager = mng;
    }

    public void OnClick()
    {
        manager.SelectCard(gameObject);
    }
}