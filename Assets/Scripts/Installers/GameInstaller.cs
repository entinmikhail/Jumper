using Character;
using GameModels;
using GameModels.StateMachine;
using Gameplay;
using Platforms;
using Popups;
using Services;
using UIControllers;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private PlatformCreator _platformCreator;
    [SerializeField] private PopupService _popupService;
    [SerializeField] private GameAnimatorController _gameAnimatorController;
    [SerializeField] private NotificationService _notificationService;
    [SerializeField] private PlayerUiController _playerUiController;
    [SerializeField] private LoadingCurtainsViewer _loadingCurtainsViewer;

    public override void InstallBindings()
    {
        Container.Bind<ICharacterMover>().FromInstance(_characterMover).AsSingle();
        Container.Bind<IPlatformCreator>().FromInstance(_platformCreator).AsSingle();
        Container.Bind<IPopupService>().FromInstance(_popupService).AsSingle();
        Container.Bind<IGameAnimatorController>().FromInstance(_gameAnimatorController).AsSingle();
        Container.Bind<INotificationService>().FromInstance(_notificationService).AsSingle();
        Container.Bind<IUiBus>().FromInstance(_playerUiController).AsSingle();
        Container.Bind<ILoadingCurtainsViewer>().FromInstance(_loadingCurtainsViewer).AsSingle();
        
        Container.Bind<IPlatformService>().To<PlatformService>().AsSingle();
        Container.Bind<IGameStateController>().To<GameStateController>().AsSingle();
        Container.Bind<IGameLoopStateMachine>().To<GameLoopStateMachine>().AsSingle();
        
        BindStates();
    }

    private void BindStates()
    {
        Container.Bind<IPrepareGameState>().To<PrepareGameState>().AsSingle();
        Container.Bind<IMainGameState>().To<MainGameState>().AsSingle();
        Container.Bind<IContinueGameState>().To<ContinueGameState>().AsSingle();
        Container.Bind<IWinGameState>().To<WinGameState>().AsSingle();
        Container.Bind<ILoseGameState>().To<LoseGameState>().AsSingle();
        Container.Bind<IBonusGameState>().To<BonusGameState>().AsSingle();
        Container.Bind<ILoadingState>().To<LoadingState>().AsSingle();
    }
}