using System;
using Character;
using Configs;
using Platforms;
using Popups;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IStartGameState : IState
    {
    }

    public class StartGameState : IStartGameState
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private IPopupService _popupService;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPlatformService _platformService;
        [Inject] private IAnimationDurationConfig _animationDurationConfig;

        public void Enter()
        {
            _gameModel.Jumped += OnJumped;
            _gameModel.BonusPiked += OnBonusPiked;
        }

        private void OnJumped(int index)
        {
            _platformService.TryAddPlatformObjectByData(index + 2);
            _characterMover.SetNumberPlatform(index, _animationDurationConfig.DefaultJumpAnimationTime);
        }

        private void OnBonusPiked(int arg, BonusType bonusType)
        {
            switch (bonusType)
            {
                case BonusType.Non:
                    break;
                case BonusType.ExtraJump:
                    _popupService.ShowPopup(PopupType.ExtraJumpBonusPopup);
                    break;
                case BonusType.ExtraFactor:
                    _popupService.ShowPopup(PopupType.ExtraFactorBonusPopup);
                    break;
                case BonusType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        public void Exit()
        {
            _gameModel.Jumped -= OnJumped;
            _gameModel.BonusPiked -= OnBonusPiked;

        }
    }
}