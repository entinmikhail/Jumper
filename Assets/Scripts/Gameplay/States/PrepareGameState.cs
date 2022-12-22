using Character;
using Configs;
using Platforms;
using Services;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IPrepareGameState : IState
    {
    }

    public class PrepareGameState : IPrepareGameState
    {
        private const int DefaultPlatformCount = 3;

        [Inject] private ILoadingCurtainsViewer _loadingCurtainsViewer;
        [Inject] private IPlatformService _platformService;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameConfigs _gameConfigs;

        public void Enter()
        {
            // _loadingCurtainsViewer.Enable();
            _characterMover.RefreshCharacter();
            _gameStorage.CurrentFactor = _gameConfigs.DefaultFactor;
        }

        private void GeneratePlatforms()
        {
            for (int i = 1; i <= DefaultPlatformCount; i++)
                _platformService.TryAddPlatformObjectByData(i);
        }

        public void Exit()
        {
            _platformService.ResetPlatformsData();
            GeneratePlatforms();
            // _loadingCurtainsViewer.Disable();
        }
    }
}