using UnityEngine;

public class CameraDragPan : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float dragSensitivity = 0.01f;
    [SerializeField] private bool invert = true;

    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;

    private bool _dragging;
    private Vector3 _lastPointer;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (PointerDown())
        {
            _dragging = true;
            _lastPointer = GetPointerPosition();
        }
        else if (PointerUp())
        {
            _dragging = false;
        }

        if (!_dragging) return;

        Vector3 currentPointer = GetPointerPosition();
        Vector3 delta = currentPointer - _lastPointer;

        float sign = invert ? -1f : 1f;

        Vector3 move = new Vector3(delta.x * dragSensitivity * sign, 0f, 0f);

        transform.position += move;

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        transform.position = p;

        _lastPointer = currentPointer;
    }

    private Vector3 GetPointerPosition()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;

        return Input.mousePosition;
    }

    private bool PointerDown()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Began;

        return Input.GetMouseButtonDown(0);
    }

    private bool PointerUp()
    {
        if (Input.touchCount > 0)
        {
            var ph = Input.GetTouch(0).phase;
            return ph == TouchPhase.Ended || ph == TouchPhase.Canceled;
        }

        return Input.GetMouseButtonUp(0);
    }
}