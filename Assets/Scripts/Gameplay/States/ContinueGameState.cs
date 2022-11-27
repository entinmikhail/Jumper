using Character;
using Platforms;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IContinueGameState : IState
    {
    }

    public class ContinueGameState : IContinueGameState
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPlatformService _platformService;

        public void Enter()
        {
            _gameModel.Jumped += OnJumped;
            
            for (int i = _gameModel.CurrentAltitude - 2; i < _gameModel.CurrentAltitude + 2; i++)
                _platformService.TryAddPlatformObjectByData(i);
            
            _characterMover.SetNumberPlatform(_gameModel.CurrentAltitude);
        }

        private void OnJumped(int index)
        {
            _platformService.TryAddPlatformObjectByData(index + 2);
            _characterMover.SetNumberPlatform(index);
        }

        public void Exit()
        {
            _gameModel.Jumped -= OnJumped;
        }
    }
}