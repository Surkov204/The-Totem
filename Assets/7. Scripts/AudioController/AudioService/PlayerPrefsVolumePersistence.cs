using UnityEngine;

public class PlayerPrefsVolumePersistence : IVolumePersistence
{
    private const string MusicKey = "MusicVolume";
    private const string SfxKey = "SfxVolume";

    public float LoadMusicVolume() =>
        PlayerPrefs.GetFloat(MusicKey, 1f);

    public float LoadSfxVolume() =>
        PlayerPrefs.GetFloat(SfxKey, 1f);

    public void SaveMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    public void SaveSfxVolume(float value)
    {
        PlayerPrefs.SetFloat(SfxKey, value);
    }
}