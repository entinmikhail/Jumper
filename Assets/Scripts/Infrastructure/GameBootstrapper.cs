using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    [Inject] private IGame _game;
    
    private void Awake()
    {
        _game.Start();
        DontDestroyOnLoad(this);
    }
}