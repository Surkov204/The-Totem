using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindCardSelectionManager();
    }

    private void BindCardSelectionManager()
    {
        Container.Bind<CardSelectionManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<GameSpeedController>().
            FromComponentInHierarchy().
            AsSingle();

        Container.Bind<LevelRunner>()
        .FromComponentInHierarchy()
        .AsSingle();
    }
}