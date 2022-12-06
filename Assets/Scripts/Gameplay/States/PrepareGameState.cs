using Character;
using Platforms;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IPrepareGameState : IState
    {
    }

    public class PrepareGameState : IPrepareGameState
    {
        private const int DefaultPlatformCount = 2;
        
        [Inject] private IPlatformService _platformService;
        [Inject] private ICharacterMover _characterMover;

        public void Enter()
        {
            GeneratePlatforms();
            _characterMover.RefreshCharacter();
        }

        private void GeneratePlatforms()
        {
            for (int i = 1; i <= DefaultPlatformCount; i++)
                _platformService.TryAddPlatformObjectByData(i);
        }

        public void Exit()
        {
            
        }
    }
}