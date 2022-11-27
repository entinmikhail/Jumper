using Character;
using Platforms;
using UnityEngine;
using Zenject;

public interface IGameStateController
{
    void Start();
    void Restart();
}

public class GameStateController : IGameStateController
{
    [Inject] private ICharacterMover _characterMover;
    [Inject] private IPlatformService _platformService;

    public void Start()
    {
        GeneratePlatforms();
    }
        
    public void Restart()
    {
        _characterMover.ResetCharacterPosition();
        _platformService.ResetPlatformsData();
        GeneratePlatforms();
    }
        
    public void GeneratePlatforms()
    {
        _platformService.TryAddPlatformObjectByData(1, new PlatformData(false, BonusType.Non));
        _platformService.TryAddPlatformObjectByData(2, new PlatformData(false, BonusType.ExtraMultiplayer));
        _platformService.TryAddPlatformObjectByData(3, new PlatformData(false, BonusType.Non));
        _platformService.TryAddPlatformObjectByData(4, new PlatformData(false, BonusType.ExtraJump));
        _platformService.TryAddPlatformObjectByData(5, new PlatformData(true, BonusType.Non));
    }
}