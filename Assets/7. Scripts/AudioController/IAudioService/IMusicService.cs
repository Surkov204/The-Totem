using UnityEngine;

public interface IMusicService
{
    void Play(AudioClip clip, bool loop = true);
    void Stop();
    void SetVolume(float value);
    float GetVolume();
}