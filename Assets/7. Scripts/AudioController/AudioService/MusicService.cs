using UnityEngine;

public class MusicService : IMusicService
{
    private readonly AudioSource _musicSource;

    public MusicService(AudioSource musicSource)
    {
        _musicSource = musicSource;
        Debug.Log("[MusicService] CONSTRUCTED");
    }

    public void Play(AudioClip clip, bool loop = true)
    {
        if (clip == null)
        {
            Debug.Log("[MusicService] Play called with NULL clip");
            return;
        }
        if (_musicSource.clip == clip && _musicSource.isPlaying)
            return;

        _musicSource.clip = clip;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void Stop()
    {
        _musicSource.Stop();
    }

    public void SetVolume(float value)
    {
        _musicSource.volume = value;
    }

    public float GetVolume() => _musicSource.volume;
}