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
    [SerializeField] private FakeServer _fakeServer;
    [SerializeField] private AnimationDurationConfig _animationDurationConfig;

    public override void InstallBindings()
    {
        Container.Bind<ICoroutineRunner>().FromInstance(_coroutineRunner).AsSingle();
        Container.Bind<IServer>().FromInstance(_fakeServer).AsSingle();
        Container.Bind<IAnimationDurationConfig>().FromInstance(_animationDurationConfig).AsSingle();

        Container.Bind<IGame>().To<Game>().AsSingle();
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        Container.Bind<IGameModel>().To<GameModel>().AsSingle();
        Container.Bind<IAccountModel>().To<AccountModel>().AsSingle();

        BindStates();
    }

    private void BindStates()
    {
        Container.Bind<IBootstrapState>().To<BootstrapState>().AsSingle();
        Container.Bind<ILoadLevelState>().To<LoadLevelState>().AsSingle();
        Container.Bind<IGameLoopState>().To<GameLoopState>().AsSingle();
    }
}