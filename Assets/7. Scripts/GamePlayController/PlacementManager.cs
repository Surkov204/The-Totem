using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;


    private GameObject currentPrefab;
    private GameObject ghost;
    private CardPlacement currentCard;

    private Camera cam;

    public GridHoverController hover;
    public GridManager grid;

    [SerializeField] private LayerMask cellLayer;

    private bool waitingForRelease;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    private void Update()
    {
        Vector2 screenPos;
        bool isPressed = false;
        bool isReleased = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        screenPos = Input.mousePosition;
        isPressed = Input.GetMouseButtonDown(0);
        isReleased = Input.GetMouseButtonUp(0);
#else
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            screenPos = t.position;
            isPressed = t.phase == TouchPhase.Began;
            isReleased = t.phase == TouchPhase.Ended;
        }
        else return;
#endif

        HandlePlacement(screenPos, isPressed, isReleased);
    }

    private void HandlePlacement(Vector2 screenPos, bool isPressed, bool isReleased)
    {
        if (currentPrefab != null)
        {
            UpdateGhost(screenPos);

            if (waitingForRelease)
            {
                if (isReleased)
                {
                    waitingForRelease = false;
                }
                return;
            }

            if (isReleased)
            {
                TryPlaceAtScreenPos(screenPos);
            }
        }
        else
        {
            hover.HideAll();
        }
    }

    private void UpdateGhost(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (grid.TryWorldToCell(hit.point, out Vector2Int cell))
            {
                Vector3 pos = grid.CellToWorldCenter(cell);

                if (ghost != null)
                    ghost.transform.position = pos;

                hover.Show(cell);
            }
            else
            {
                hover.HideAll();
            }
        }
    }

    public void BeginPlacement(GameObject prefab, CardPlacement card)
    {
        if (currentPrefab != null) return;

        currentPrefab = prefab;
        currentCard = card;
        waitingForRelease = true;

        ghost = Instantiate(prefab,
            new Vector3(0, -10000, 0),
            Quaternion.Euler(0f, 90f, 0f));

        foreach (var c in ghost.GetComponentsInChildren<Collider>())
            c.enabled = false;

        SetGhostAlpha(ghost, 0.5f);
    }

    public void TryPlaceAtScreenPos(Vector2 screenPos)
    {
        if (currentPrefab == null) return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (!plane.Raycast(ray, out float distance))
        {
            currentCard?.ForceDeselect();
            Cleanup();
            return;
        }

        Vector3 worldPoint = ray.GetPoint(distance);

        if (!grid.TryWorldToCell(worldPoint, out Vector2Int cellPos))
        {
            currentCard?.ForceDeselect();
            Cleanup();
            return;
        }

        if (!grid.IsInside(cellPos) || grid.IsOccupied(cellPos))
        {
            currentCard?.ForceDeselect();
            Cleanup();
            return;
        }

        Vector3 pos = grid.CellToWorldCenter(cellPos);
        pos.y = -1f;

        bool isExploder =
            currentPrefab.GetComponent<ExplodeComponent>() != null;

        GameObject defender;

        if (isExploder)
        {
            defender = Instantiate(
                currentPrefab,
                Vector3.zero,
                Quaternion.Euler(0f, 90f, 0f)
            );

            defender.GetComponent<ExplodeComponent>()
                    .PlayDropAndExplode(pos);
        }
        else
        {
            defender = Instantiate(
                currentPrefab,
                pos,
                Quaternion.Euler(0f, 90f, 0f)
            );
        }

        grid.Occupy(cellPos, defender);

        var laneComp = defender.GetComponent<DefenderLane>();
        if (laneComp != null)
            laneComp.SetLane(cellPos.y);

        DefenderManager.Instance.Register(
            defender.GetComponent<BaseDefender>(),
            cellPos.y
        );

        currentCard?.NotifyPlaced();
        Cleanup();
    }

    private void Cleanup()
    {
        currentPrefab = null;
        currentCard = null;

        if (ghost != null)
            Destroy(ghost);

        hover.HideAll();
    }

    private void SetGhostAlpha(GameObject go, float alpha)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
        {
            foreach (var mat in r.materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
            }
        }
    }
}
