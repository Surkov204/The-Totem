using Zenject;
using UnityEngine;
using JS;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private UIManager uiManager;

    public override void InstallBindings()
    {
        Container.Bind<IUIService>()
            .FromInstance(uiManager)
            .AsSingle();
    }
}
