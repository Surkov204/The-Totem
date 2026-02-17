using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private bool isSelected;

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
        TryBeginPlacement();
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
}
