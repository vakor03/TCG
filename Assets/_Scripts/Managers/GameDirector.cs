using System;

namespace _Scripts.Managers
{
    public class GameDirector
    {
        private GameState _gameState;

        public bool IsGameStarted => _gameState == GameState.Started;
        public bool IsGamePaused => _gameState == GameState.Paused;
        public bool IsGameFinished => _gameState == GameState.Finished;
        public bool IsGameInitializing => _gameState == GameState.Initializing;
        public bool IsGameInitialized => _gameState == GameState.Initialized;

        public event Action OnGameStarted;
        public event Action OnGamePaused;
        public event Action OnGameFinished;
        public event Action OnGameInitializing;
        public event Action OnGameInitialized;

        private void SwitchGameState(GameState gameState)
        {
            _gameState = gameState;
            switch (gameState)
            {
                case GameState.Started:
                    OnGameStarted?.Invoke();
                    break;
                case GameState.Paused:
                    OnGamePaused?.Invoke();
                    break;
                case GameState.Finished:
                    OnGameFinished?.Invoke();
                    break;
                case GameState.Initializing:
                    OnGameInitializing?.Invoke();
                    break;
                case GameState.Initialized:
                    OnGameInitialized?.Invoke();
                    break;
            }
        }

        public void StartGame()
        {
            SwitchGameState(GameState.Started);
        }

        public void PauseGame()
        {
            SwitchGameState(GameState.Paused);
        }

        public void FinishGame()
        {
            SwitchGameState(GameState.Finished);
        }

        private enum GameState
        {
            Initializing,
            Initialized,
            Started,
            Paused,
            Finished
        }
    }
}