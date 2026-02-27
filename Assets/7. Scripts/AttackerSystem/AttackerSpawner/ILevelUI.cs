public interface ILevelUI
{
    void OnLevelStart(float totalDuration);
    void OnLevelTime(float currentTime);
    void OnLevelEnd();
    void OnHugeWaveStart();
    void OnHugeWaveEnd();
}