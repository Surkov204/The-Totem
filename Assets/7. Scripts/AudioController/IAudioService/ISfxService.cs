using UnityEngine;

public interface ISfxService
{
    void Play(SoundFXLibrary.SoundFXName name);
    void PlayOneShot(AudioClip clip);
    void PlayLoop(SoundFXLibrary.SoundFXName name);
    void StopLoop(SoundFXLibrary.SoundFXName name);
    void SetVolume(float value);
    float GetVolume();
}