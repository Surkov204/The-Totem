using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public enum CardMode
{
    Selection,
    InSlot,
    Gameplay
}

public class CardPlacement : MonoBehaviour,
    IPointerDownHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private static CardPlacement currentSelected;

    [Header("Data")]
    [SerializeField] private DefenderData data;

    [Header("Visual")]
    [SerializeField] private Image cardImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    [SerializeField] private CardCooldown cooldown;

    [SerializeField] private CardMode mode = CardMode.Gameplay;

    private CardSelectionManager selectionManager;

    private bool isSelected;
    private bool isInSlot;
    private CardSlotUI parentSlot;


    private void Awake()
    {
        if (!cardImage)
            cardImage = GetComponent<Image>();

        if (!cooldown)
            cooldown = GetComponent<CardCooldown>();
    }

    private void Start()
    {
        if (data != null && cooldown != null) {
            cooldown.Init(data.placementCooldown);
        }
    }

    private void Update()
    {
        UpdateVisualState();
    }

    [Inject]
    public void Construct(CardSelectionManager manager)
    {
        selectionManager = manager;
    }

    public void SetMode(CardMode newMode)
    {
        mode = newMode;

        if (mode == CardMode.Selection)
        {
            if (cooldown != null)
                cooldown.enabled = false;
        }
        else
        {
            if (cooldown != null)
                cooldown.enabled = true;
        }
    }

    private void UpdateVisualState()
    {
        if (data == null) return;

        bool canUse =
            cooldown.IsReady() &&
            SunManager.Instance.CanAfford(data.cost);

        if (!canUse)
            cardImage.color = Color.gray;
        else if (isSelected)
            cardImage.color = selectedColor;
        else
            cardImage.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Card Clicked");

        switch (mode)
        {
            case CardMode.Selection:
                selectionManager.SelectCard(gameObject);
                break;

            case CardMode.InSlot:
                if (parentSlot != null)
                    selectionManager.RemoveLastSelected();
                break;

            case CardMode.Gameplay:
                TryBeginPlacement();
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TryBeginPlacement();
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        PlacementManager.Instance.TryPlaceAtScreenPos(eventData.position);
        Deselect();
    }

    private void TryBeginPlacement()
    {
        if (data == null) return;
        if (!cooldown.IsReady()) return;
        if (mode != CardMode.Gameplay)
            return;
        if (!SunManager.Instance.CanAfford(data.cost)) return;

        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.ForceDeselect();
        }

        currentSelected = this;

        Select();

        EventSystem.current.SetSelectedGameObject(null);

        PlacementManager.Instance.BeginPlacement(data.prefab, this);
    }

    public void NotifyPlaced()
    {
        SunManager.Instance.Spend(data.cost);

        cooldown.TriggerCooldown();
        Deselect();
    }

    public void ForceDeselect()
    {
        isSelected = false;

        if (currentSelected == this)
            currentSelected = null;
    }

    private void Select()
    {
        isSelected = true;
    }

    private void Deselect()
    {
        isSelected = false;

        if (currentSelected == this)
            currentSelected = null;
    }

    public DefenderData GetData()
    {
        return data;
    }

    public void SetData(DefenderData newData)
    {
        data = newData;
    }

    public void SetInSlot(bool value)
    {
        isInSlot = value;
    }

    public void SetParentSlot(CardSlotUI slot)
    {
        parentSlot = slot;
    }
}
