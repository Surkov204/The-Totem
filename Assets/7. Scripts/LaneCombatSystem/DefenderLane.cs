using UnityEngine;

public class DefenderLane : MonoBehaviour
{
    public int Lane { get; private set; } = -1;

    public void SetLane(int lane)
    {
        Lane = lane;
    }
}