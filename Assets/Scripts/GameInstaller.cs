using Character;
using Platforms;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private PlatformCreator _platformCreator;

    public override void InstallBindings()
    {
        Container.Bind<ICharacterMover>().FromInstance(_characterMover).AsSingle();
        Container.Bind<IPlatformCreator>().FromInstance(_platformCreator).AsSingle();
        Container.Bind<IPlatformService>().To<PlatformService>().AsSingle();
    }
}