using System;
using UnityEngine;
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
        public GameState GameState { get; set; } = GameState.LoadingState;
        
        public void SetGameState(GameState newGameState)
        {
            GameState = newGameState;
            GameStateChanged?.Invoke(newGameState);
        }
    }
}