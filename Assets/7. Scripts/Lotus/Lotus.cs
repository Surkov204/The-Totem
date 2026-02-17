using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Lotus : MonoBehaviour
{
    [SerializeField] private int value = 25;
    [SerializeField] private float lifeTime = 30f;
    [SerializeField] private float growDuration = 0.4f;
    [SerializeField] private float flyDuration = 3f;

    [SerializeField] private LayerMask lotusLayer;

    private ResourceGenerator producer;
    private bool isFlying;
    private Transform targetUI;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        transform.localScale = Vector3.zero;

        transform.DOScale(new Vector3(2f, 2f, 2f), growDuration)
            .SetEase(Ease.OutBack);

        Destroy(gameObject, lifeTime);
    }

    public void SetTargetUI(Transform uiTarget)
    {
        targetUI = uiTarget;
    }

    private void Update()
    {
        if (isFlying) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            TryRaycast(Input.mousePosition);
#else
    if (Input.touchCount > 0 &&
        Input.GetTouch(0).phase == TouchPhase.Began)
        TryRaycast(Input.GetTouch(0).position);
#endif
    }

    public void SetProducer(ResourceGenerator gen)
    {
        producer = gen;
    }

    private void TryRaycast(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, lotusLayer))
        {
            Lotus lotus = hit.collider.GetComponentInParent<Lotus>();

            if (lotus == this)
            {
                Collect();
            }
        }
    }

    private void OnDestroy()
    {
        if (!isFlying)
        {
            NotifyProducer();
        }
    }

    private void NotifyProducer()
    {
        if (producer != null)
            producer.OnLotusCollected();
    }

    private void Collect()
    {
        if (targetUI == null) return;

        isFlying = true;

        transform.DORotate(
            new Vector3(0f, 360f, 0f),
            flyDuration,
            RotateMode.FastBeyond360
        ).SetEase(Ease.Linear);

        transform.DOMove(targetUI.position, flyDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                SunManager.Instance.Add(value);

                NotifyProducer();
                Destroy(gameObject);
            });
    }
}
