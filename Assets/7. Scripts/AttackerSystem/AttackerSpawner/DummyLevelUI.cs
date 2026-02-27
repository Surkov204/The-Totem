using UnityEngine;

public class DummyLevelUI : MonoBehaviour, ILevelUI
{
    public void OnLevelStart(float totalDuration) => Debug.Log($"Level start, duration={totalDuration}s");
    public void OnLevelTime(float t) { }
    public void ShowHugeWaveApproaching() => Debug.Log("Huge Wave Approaching!");
    public void OnHugeWaveStart() => Debug.Log("HUGE WAVE START!");
    public void OnHugeWaveEnd() => Debug.Log("Huge wave end.");
    public void OnLevelEnd() => Debug.Log("Level end.");
}