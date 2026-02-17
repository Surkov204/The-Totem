using UnityEngine;

[ExecuteAlways] 
[RequireComponent(typeof(Camera))]
public class CameraAspectScaler : MonoBehaviour
{
    public float designSize = 5f;
    public float ipadSize = 7.6f;

    private Camera cam;
    private float lastAspect;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        UpdateCamera();
    }

    void Update()
    {
        float aspect = (float)Screen.width / Screen.height;

        if (!Mathf.Approximately(aspect, lastAspect))
        {
            UpdateCamera();
        }
    }

    void UpdateCamera()
    {
        if (!cam)
            cam = GetComponent<Camera>();

        float aspect = (float)Screen.width / Screen.height;
        lastAspect = aspect;

        // iPad ~ 4:3
        if (aspect > 1.2f && aspect < 1.4f)
        {
            cam.orthographicSize = ipadSize;
        }
        else
        {
            cam.orthographicSize = designSize;
        }
    }
}
