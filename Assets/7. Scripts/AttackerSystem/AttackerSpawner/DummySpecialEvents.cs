using UnityEngine;

public class DummySpecialEvents : MonoBehaviour, ISpecialEventReceiver
{
    public void OnSpecialEvent(SpecialWorldEvent e)
    {
        Debug.Log($"SpecialEvent: type={e.type}, time={e.triggerTime}, lane={e.lane}, intParam={e.intParam}");
    }
}