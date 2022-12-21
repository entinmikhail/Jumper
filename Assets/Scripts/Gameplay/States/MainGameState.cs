using System;
using System.Collections.Generic;
using Character;
using Configs;
using Platforms;
using Popups;
using UnityEditor.Search;
using UnityEngine;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IMainGameState : IState
    {
    }

    public class MainGameState : IMainGameState
    {
        [Inject] private IGameHandler _jumpInvoker;
        [Inject] private IPopupService _popupService;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPlatformService _platformService;
        [Inject] private IAnimationDurationConfig _animationDurationConfig;

        private readonly Queue<int> _numberOfPlatforms = new();
        public void Enter()
        {
            _numberOfPlatforms.Clear();
            _jumpInvoker.Jumped += OnJumped;
            _characterMover.MoveEnd += OnMoveEnd;
        }

        private void OnMoveEnd()
        {
            if (_numberOfPlatforms.Count > 0)
                _numberOfPlatforms.Dequeue();
            
            if (_numberOfPlatforms.Count > 0)
                OnJumped(_numberOfPlatforms.Peek(), null);
        }

        private void OnJumped(int index, string bonusType)
        {
            _numberOfPlatforms.Enqueue(index);
            if (_numberOfPlatforms.Peek() != index)
                return;
            
            _platformService.TryAddPlatformObjectByData(index + 3);
            _characterMover.SetNumberPlatform(index, _animationDurationConfig.DefaultJumpAnimationTime);

            if (_platformService.TryGetPlatformContainer(index, out var platformContainer))
            {
                switch (bonusType)
                {
                    case "PLUS1":
                        platformContainer.SetBonus(BonusType.ExtraJump);
                        break;
                    case "X2":
                        platformContainer.SetBonus(BonusType.ExtraFactor);
                        break;
                    case null:
                        break;
                }
            }

            switch (bonusType)
            {
                case "PLUS1":
                    _popupService.ShowPopup(PopupType.ExtraJumpBonusPopup);
                    break;
                case "X2":
                    _popupService.ShowPopup(PopupType.ExtraFactorBonusPopup);
                    break;
                case null:
                    break;
            }
        }

        public void Exit()
        {
            _jumpInvoker.Jumped -= OnJumped;
            _characterMover.MoveEnd -= OnMoveEnd;

        }
    }
}