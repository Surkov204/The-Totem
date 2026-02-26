using UnityEngine;
using Zenject;

public class CardPoolSpawner : MonoBehaviour
{
    [SerializeField] private CardPrefabDatabase database;
    [SerializeField] private Transform poolParent;
    private DiContainer container;

    private void Start()
    {
        LoadCards();
    }

    [Inject]
    public void Construct(DiContainer di)
    {
        container = di;
    }

    private void LoadCards()
    {
        foreach (var prefab in database.cardPrefabs)
        {
            GameObject card = container.InstantiatePrefab(prefab, poolParent);

            var placement = card.GetComponentInChildren<CardPlacement>();
           
            placement.SetMode(CardMode.Selection);
        }
    }
}