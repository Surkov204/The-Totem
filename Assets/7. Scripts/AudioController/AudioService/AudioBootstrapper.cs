using UnityEngine.SceneManagement;
using Zenject;

public class AudioBootstrapper : IInitializable
{
    private readonly IAudioService _audio;

    public AudioBootstrapper(IAudioService audio)
    {
        _audio = audio;
    }

    public void Initialize()
    {
        _audio.PlayMusicForScene(SceneManager.GetActiveScene().name);
    }
}