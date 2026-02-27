public interface IAudioService
{
    void PlayMusicForScene(string sceneName);

    void PlaySFX(SoundFXLibrary.SoundFXName soundName);
    void PlayLoop(SoundFXLibrary.SoundFXName soundName);
    void StopLoop(SoundFXLibrary.SoundFXName soundName);

    void SetMusicVolume(float value);
    void SetSfxVolume(float value);

    float GetMusicVolume();
    float GetSfxVolume();
}