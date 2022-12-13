using Character;
using GameModels.StateMachine;
using UIControllers;
using Zenject;

namespace Gameplay
{
    public interface IBonusGameState : IState
    {
    }

    public class BonusGameState : IBonusGameState
    {
        [Inject] private IGameAnimatorController _gameAnimatorController;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
        [Inject] private ICharacterMover _characterMover;

        public void Enter()
        {
            _gameAnimatorController.PlayBonusJump();
            _coroutineRunner.StartAfterDelay(3, () =>
            {
                _gameLoopStateMachine.Enter<StartGameState>();
            });
            
        }

        public void Exit()
        {
            _characterMover.SetCharacterToBonusPosition();
        }
    }
}