using GameModels.StateMachine;
using Server;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private CoroutineRunner _coroutineRunner;
    [SerializeField] private FakeServer _fakeServer;

    public override void InstallBindings()
    {
        Container.Bind<ICoroutineRunner>().FromInstance(_coroutineRunner).AsSingle();

        Container.Bind<IGame>().To<Game>().AsSingle();
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        Container.Bind<IFakeServer>().FromInstance(_fakeServer).AsSingle();
        
        Container.Bind<IBootstrapState>().To<BootstrapState>().AsSingle();
        Container.Bind<IGameplayState>().To<GameplayState>().AsSingle();
        Container.Bind<IStartGameState>().To<StartGameState>().AsSingle();
        Container.Bind<IContinueGameState>().To<ContinueGameState>().AsSingle();
        Container.Bind<ILoadLevelState>().To<LoadLevelState>().AsSingle();
    }
}