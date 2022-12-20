using System;
using Zenject;

namespace GameModels
{
    public interface IGameModel
    {
        event Action<GameState> GameStateChanged;
        GameState GameState { get; set; }
        void SetGameState(GameState newGameState);
    }

    public class GameModel : IGameModel
    {
        public event Action<GameState> GameStateChanged;
        public GameState GameState { get; set; }
        
        [Inject] private IAccountModel _accountModel;

        
        public void SetGameState(GameState newGameState)
        {
            GameState = newGameState;
            GameStateChanged?.Invoke(newGameState);
        }
    }
}