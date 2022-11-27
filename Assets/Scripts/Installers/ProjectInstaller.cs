using GameModels.StateMachine;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private CoroutineRunner _coroutineRunner;

    public override void InstallBindings()
    {
        Container.Bind<ICoroutineRunner>().FromInstance(_coroutineRunner).AsSingle();
        
        Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }
}