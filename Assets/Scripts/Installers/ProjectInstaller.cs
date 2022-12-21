using Configs;
using GameModels;
using GameModels.StateMachine;
using Infrastructure.States;
using Server;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private CoroutineRunner _coroutineRunner;
    [SerializeField] private AnimationDurationConfig _animationDurationConfig;

    public override void InstallBindings()
    {
        Container.Bind<ICoroutineRunner>().FromInstance(_coroutineRunner).AsSingle();
        Container.Bind<IAnimationDurationConfig>().FromInstance(_animationDurationConfig).AsSingle();

        Container.Bind<IGame>().To<Game>().AsSingle();
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        Container.Bind<IGameConfigs>().To<GameConfigs>().AsSingle();


        Container.Bind<IAccountModel>().To<AccountModel>().AsSingle();
        Container.Bind<IJumperServerApi>().To<JumperJumperServerApi>().AsSingle();
        Container.Bind<IGameModel>().To<GameModel>().AsSingle();
        Container.Bind<IGameStorage>().To<GameStorage>().AsSingle();
        Container.Bind<IGameController>().To<GameController>().AsSingle();
        Container.Bind<IGameHandler>().To<GameHandler>().AsSingle();

        BindStates();
        
        // var gameHandler = new GameHandler();
        // Container.Inject(gameHandler);
        // Container.Bind<IJumpInvoker>().FromInstance(gameHandler).AsSingle();
        // Container.Bind<IGameHandler>().FromInstance(gameHandler).AsSingle();
    }

    private void BindStates()
    {
        Container.Bind<IBootstrapState>().To<BootstrapState>().AsSingle();
        Container.Bind<ILoadLevelState>().To<LoadLevelState>().AsSingle();
        Container.Bind<IGameLoopState>().To<GameLoopState>().AsSingle();
    }
}