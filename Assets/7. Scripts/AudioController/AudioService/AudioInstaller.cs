using UnityEngine;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private SceneMusicConfig musicConfig;
    [SerializeField] private SoundFXLibrary sfxLibrary;

    public override void InstallBindings()
    {
        Debug.Log("[AudioInstaller] InstallBindings CALLED");

        Container.Bind<IVolumePersistence>()
            .To<PlayerPrefsVolumePersistence>()
            .AsSingle();

        Container.Bind<IMusicService>()
            .To<MusicService>()
            .AsSingle()
            .WithArguments(musicSource);

        Container.Bind<ISfxService>()
            .To<SfxService>()
            .AsSingle()
            .WithArguments(sfxSource, sfxLibrary);

        Container.Bind<IAudioService>()
            .To<AudioService>()
            .AsSingle()
            .WithArguments(musicConfig);
        Container.BindInterfacesTo<AudioBootstrapper>().AsSingle();
    }
}