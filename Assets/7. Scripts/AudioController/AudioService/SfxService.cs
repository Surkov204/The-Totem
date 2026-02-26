using System.Collections.Generic;
using UnityEngine;
using static SoundFXLibrary;

public class SfxService : ISfxService
{
    private readonly AudioSource _sfxSource;
    private readonly SoundFXLibrary _library;

    private readonly Dictionary<SoundFXName, AudioSource> _loops = new();

    public SfxService(AudioSource sfxSource, SoundFXLibrary library)
    {
        _sfxSource = sfxSource;
        _library = library;
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
            _sfxSource.PlayOneShot(clip);
    }

    public void PlayLoop(SoundFXName name)
    {
        if (_loops.ContainsKey(name))
        {
            if (!_loops[name].isPlaying)
                _loops[name].Play();
            return;
        }

        var clip = _library.GetClip(name);
        if (clip == null) return;

        GameObject go = new GameObject($"Loop_{name}");
        Object.DontDestroyOnLoad(go);

        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.loop = true;
        src.volume = _sfxSource.volume;
        src.Play();

        _loops[name] = src;
    }

    public void Play(SoundFXName name)
    {
        var clip = _library.GetClip(name);
        if (clip != null)
            _sfxSource.PlayOneShot(clip);
    }

    public void StopLoop(SoundFXName name)
    {
        if (!_loops.ContainsKey(name)) return;

        _loops[name].Stop();
    }

    public void SetVolume(float value)
    {
        _sfxSource.volume = value;

        foreach (var src in _loops.Values)
            src.volume = value;
    }

    public float GetVolume() => _sfxSource.volume;
}