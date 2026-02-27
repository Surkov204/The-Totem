using UnityEngine;

public class FloatingRotateY : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 60f; 

    [Header("Floating")]
    [SerializeField] private float floatAmplitude = 0.2f; 
    [SerializeField] private float floatFrequency = 1f;  

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Rotate();
        Float();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void Float()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}