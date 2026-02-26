using UnityEngine;
using UnityEngine.EventSystems;

public class WorldClickTapRaycaster : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask hitMask; 
    [SerializeField] private float maxDistance = 500f;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (!TryGetPress(out var screenPos)) return;
        if (IsPointerOverUI()) return;

        var ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit, maxDistance, hitMask, QueryTriggerInteraction.Ignore))
        {
            var sel = hit.collider.GetComponentInParent<LevelSelectWorld>();
            sel?.Select();
        }
    }

    private bool TryGetPress(out Vector2 pos)
    {
        if (Input.GetMouseButtonDown(0)) { pos = Input.mousePosition; return true; }

        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began) { pos = t.position; return true; }
        }

        pos = default;
        return false;
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        if (Input.GetMouseButtonDown(0))
            return EventSystem.current.IsPointerOverGameObject();

        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

        return false;
    }
}