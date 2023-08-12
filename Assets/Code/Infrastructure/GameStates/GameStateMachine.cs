using System;
using System.Collections.Generic;
using Code.Infrastructure.Services.StaticDataService;

namespace Code.Infrastructure.GameStates
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _states;
        private IGameState _currentState;
        public GameStateMachine(ICoroutineRunner coroutineRunner, IStaticDataService staticDataService, string firstSceneName, SoundDataService soundDataService)
        {
            _states = new Dictionary<Type, IGameState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, staticDataService),
                [typeof(LoadLevelState)] = new LoadLevelState(this, coroutineRunner, staticDataService, firstSceneName, soundDataService),
                [typeof(GameLoopState)] = new GameLoopState(this)
            };
        }

        public void EnterState<T>() where T : IGameState
        {
            if (_states.TryGetValue(typeof(T), out var state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }
    }
}