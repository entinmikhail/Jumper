using Character;
using GameModels;
using GameModels.StateMachine;
using Gameplay;
using Platforms;
using Popups;
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
        Container.Bind<IGameLoopStateMachine>().To<GameLoopStateMachine>().AsSingle();
        
        BindStates();
    }

    private void BindStates()
    {
        Container.Bind<IPrepareGameState>().To<PrepareGameState>().AsSingle();
        Container.Bind<IStartGameState>().To<StartGameState>().AsSingle();
        Container.Bind<IContinueGameState>().To<ContinueGameState>().AsSingle();
        Container.Bind<IWinGameState>().To<WinGameState>().AsSingle();
        Container.Bind<ILoseGameState>().To<LoseGameState>().AsSingle();
        Container.Bind<IBonusGameState>().To<BonusGameState>().AsSingle();
    }
}