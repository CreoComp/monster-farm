using Code.Infrastructure.GameStates;
using Code.Infrastructure.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private GameStateMachine _gameStateMachine;
        [SerializeField] private string _sceneToLoadName;

        private SoundDataService _soundDataService;
        [Inject]
        public void Construct(SoundDataService soundDataService)
        {
            _soundDataService = soundDataService;
            Debug.Log(_soundDataService);
        }
        
        private void Start()
        {
            _gameStateMachine = new GameStateMachine(this, new StaticDataService(), _sceneToLoadName, _soundDataService);
            _gameStateMachine.EnterState<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}