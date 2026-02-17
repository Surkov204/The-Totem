using DG.Tweening;
using UnityEngine;

public class ResourceGenerator : BaseDefender ,IResourceGenerator
{
    [SerializeField] private int value = 25;
    [SerializeField] private float interval = 5f;

    [Header("Lotus Mode")]
    [SerializeField] private bool spawnLotus;
    [SerializeField] private GameObject lotusPrefab;

    private float timer;
    private bool isShrunk;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (isShrunk) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            Generate();
            timer = 0f;
        }
    }

    public void Generate()
    {
        if (!spawnLotus)
        {
            SunManager.Instance.Add(value);
        }
        else
        {
            SpawnLotus();
        }
    }

    private void SpawnLotus()
    {

        Debug.Log("SpawnLotus called");

        if (lotusPrefab == null)
        {
            Debug.LogError("Lotus Prefab is NULL!");
            return;
        }

        isShrunk = true;

        transform.localScale = originalScale * 0.5f;

        Vector3 pos = transform.position;
        pos.y = 3f;

        GameObject lotus = Instantiate(lotusPrefab, pos, Quaternion.identity);

        Lotus lotusScript = lotus.GetComponent<Lotus>();
        lotusScript.SetTargetUI(SunManager.Instance.GetSunTarget());
        lotusScript.SetProducer(this);
    }

    public void OnLotusCollected()
    {
        isShrunk = false;
        timer = 0f;

        transform.DOKill();

        transform.DOScale(originalScale * 1.15f, 0.15f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.15f)
                    .SetEase(Ease.InOutQuad);
            });
    }

}
