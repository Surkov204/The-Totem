using UnityEngine;

public class AudioService : IAudioService
{
    private readonly IMusicService _music;
    private readonly ISfxService _sfx;
    private readonly IVolumePersistence _volume;
    private readonly SceneMusicConfig _config;

    public AudioService(
        IMusicService music,
        ISfxService sfx,
        IVolumePersistence volume,
        SceneMusicConfig config)
    {
        _music = music;
        _sfx = sfx;
        _volume = volume;
        _config = config;

        // Load saved volume
        _music.SetVolume(_volume.LoadMusicVolume());
        _sfx.SetVolume(_volume.LoadSfxVolume());
    }

    public void PlayMusicForScene(string sceneName)
    {
        var clip = _config.GetMusicForScene(sceneName);
        _music.Play(clip);
    }

    public void PlaySFX(SoundFXLibrary.SoundFXName name)
    {
        _sfx.Play(name);
    }

    public void PlayLoop(SoundFXLibrary.SoundFXName name)
        => _sfx.PlayLoop(name);

    public void StopLoop(SoundFXLibrary.SoundFXName name)
        => _sfx.StopLoop(name);

    public void SetMusicVolume(float value)
    {
        _music.SetVolume(value);
        _volume.SaveMusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        _sfx.SetVolume(value);
        _volume.SaveSfxVolume(value);
    }

    public float GetMusicVolume() => _music.GetVolume();
    public float GetSfxVolume() => _sfx.GetVolume();
}