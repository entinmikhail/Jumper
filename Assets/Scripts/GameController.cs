using System;
using Character;
using Platforms;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPlatformService _platformService;

        private void Awake()
        {
            _characterMover.PlatformBroke += OnPlatformBroke;
        }

        private void Start()
        {
            GeneratePlatforms();
        }

        private void GeneratePlatforms()
        {
            _platformService.TryAddPlatformObjectByData(1, new PlatformData(false, BonusType.Non));
            _platformService.TryAddPlatformObjectByData(2, new PlatformData(false, BonusType.ExtraMultiplayer));
            _platformService.TryAddPlatformObjectByData(3, new PlatformData(false, BonusType.ExtraJump));
            _platformService.TryAddPlatformObjectByData(4, new PlatformData(false, BonusType.Non));
            _platformService.TryAddPlatformObjectByData(5, new PlatformData(true, BonusType.Non));
        }

        private void OnPlatformBroke()
        {
            Debug.LogError("Ты лох");
        }
    }
}