using System.Collections.Generic;
using Character;
using Configs;
using Platforms;
using Popups;
using UIControllers;
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
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private IPlatformService _platformService;
        [Inject] private IButtonsLockService _buttonsLockService;
        [Inject] private IAnimationDurationConfig _animationDurationConfig;

        private readonly Queue<JumpData> _numberOfPlatforms = new();
        public void Enter()
        {
            _numberOfPlatforms.Clear();
            _jumpInvoker.Jumped += OnJumped;
            _characterMover.MoveEnd += OnMoveEnd;
        }

        private void OnMoveEnd()
        {
            if (_numberOfPlatforms.Count > 0)
            {
                var jumpData = _numberOfPlatforms.Dequeue();
                
                if (_numberOfPlatforms.Count == 0)
                    _buttonsLockService.UnlockAllButtons();
                
                _coroutineRunner.StartAfterDelay(0.5f, () =>
                {
                    
                    if (_platformService.TryGetPlatformContainer(jumpData.Index, out var platformContainer))
                        platformContainer.OnMoveEnd();
            
                    _coroutineRunner.StartAfterDelay(0.3f, () =>
                    {
                        platformContainer.PlayDisable();
                        switch (jumpData.BonusType)         
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
                        
                        if (_numberOfPlatforms.Count > 0)
                        {
                            _coroutineRunner.StartAfterDelay(0.5f, () =>
                            {
                                var jumpData = _numberOfPlatforms.Peek();
                                Jump(jumpData);
                            });
                        }
                    });
                });
            }

        }

        private void OnJumped(int index, string bonusType)
        {
            var jumpData = new JumpData(index, bonusType);
            
            _numberOfPlatforms.Enqueue(jumpData);
            
            if (_numberOfPlatforms.Peek().Index != jumpData.Index)
                return;
            
            Jump(jumpData);
        }

        private void Jump(JumpData jumpData)
        {
            _buttonsLockService.LockAllButtons();
            _platformService.TryAddPlatformObjectByData(jumpData.Index + 3);
            _characterMover.SetNumberPlatform(jumpData.Index, _animationDurationConfig.DefaultJumpAnimationTime);

            if (_platformService.TryGetPlatformContainer(jumpData.Index, out var platformContainer))
            {
                switch (jumpData.BonusType)
                    // switch ("X2")
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
        }

        public void Exit()
        {
            _jumpInvoker.Jumped -= OnJumped;
            _characterMover.MoveEnd -= OnMoveEnd;

        }

        private class JumpData
        {
            public JumpData(int index, string bonusType)
            {
                Index = index;
                BonusType = bonusType;
            }

            public int Index;
            public string BonusType;
        }
    }

}