using Character;
using GameModels.StateMachine;
using Platforms;
using Popups;
using Server;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private PlatformCreator _platformCreator;
    [SerializeField] private PopupService _popupService;

    public override void InstallBindings()
    {
        Container.Bind<ICharacterMover>().FromInstance(_characterMover).AsSingle();
        Container.Bind<IPlatformCreator>().FromInstance(_platformCreator).AsSingle();
        Container.Bind<IPopupService>().FromInstance(_popupService).AsSingle();
        
        Container.Bind<IPlatformService>().To<PlatformService>().AsSingle();
        Container.Bind<IGameStateController>().To<GameStateController>().AsSingle();
    }
}