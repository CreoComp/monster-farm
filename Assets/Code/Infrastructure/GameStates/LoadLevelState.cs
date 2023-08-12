using System.Linq;
using Code.Infrastructure.GameFactory;
using Code.Infrastructure.Services.PlayerProgressService;
using Code.Infrastructure.Services.StaticDataService;
using Code.Logic;
using Code.Logic.Buildings;
using UnityEngine;

namespace Code.Infrastructure.GameStates
{
    public class LoadLevelState : IGameState
    {
        private const string MonsterSpawnPointTag = "MonsterSpawnPoint";
        private const string IncubatorSpawnPointTag = "IncubatorSpawnPoint";
        private const string ConnectorSpawnPointTag = "ConnectorSpawnPoint";
        private const string MilitaryOfficeSpawnPoint = "MilitaryOfficeSpawnPoint";

        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SceneLoader _sceneLoader;
        private readonly string _sceneNameToLoad;

        private IPlayerProgressService _playerProgressServiceOnCurrentLevel;
        private IGameFactory _factoryOnCurrentLevel;

        private SoundDataService _soundDataService;

        public LoadLevelState(GameStateMachine stateMachine, ICoroutineRunner coroutineRunner,
            IStaticDataService staticDataService, string firstSceneName, SoundDataService soundDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
            _sceneLoader = new SceneLoader(coroutineRunner);
            _sceneNameToLoad = firstSceneName;
            _soundDataService = soundDataService;
        }
        public void Enter()
        {
            _sceneLoader.Load(_sceneNameToLoad, OnLoaded);
            _stateMachine.EnterState<GameLoopState>();
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            CreatePlayerProgress();
            var bordersSpawnTransform = FindBordersSpawnTransform();
            CreateAndInitializeFactory(bordersSpawnTransform);
            CreateHUD();
            SpawnStartMonsters();
            //SpawnStartConstructions();
        }

        private static BordersSpawnTransform FindBordersSpawnTransform()
        {
            BordersSpawnTransform bordersSpawnTransform = Object.FindObjectOfType<BordersSpawnTransform>();
            if (bordersSpawnTransform == null)
            {
                Debug.LogError("Did not find Border Spawn Transform at scene!");
            }

            return bordersSpawnTransform;
        }

        private void SpawnStartMonsters()
        {
            var spawnPoints = GameObject.FindGameObjectsWithTag(MonsterSpawnPointTag)
                .Select(gameObject => gameObject.transform);

            foreach (Transform transform in spawnPoints)
            {
                _factoryOnCurrentLevel.CreateRandomMonsterWithCheapestRareLevel(transform.position);
            }
        }

        private void SpawnStartConstructions()
        {
            var incubatorsSpawnPoints = GameObject.FindGameObjectsWithTag(IncubatorSpawnPointTag)
                .Select(gameObject => gameObject.transform);
            var connectorsSpawnPoints = GameObject.FindGameObjectsWithTag(ConnectorSpawnPointTag)
                .Select(gameObject => gameObject.transform);
            var militaryOfficesSpawnPoints = GameObject.FindGameObjectsWithTag(MilitaryOfficeSpawnPoint)
                .Select(gameObject => gameObject.transform);

            foreach (Transform transform in incubatorsSpawnPoints)
            {
                _factoryOnCurrentLevel.CreateConstruction<EggIncubation>(transform.position);
                break;
            }
            foreach (Transform transform in connectorsSpawnPoints)
            {
                _factoryOnCurrentLevel.CreateConstruction<ConnectDivideMonsters>(transform.position);
                break;

            }
            foreach (Transform transform in militaryOfficesSpawnPoints)
            {
                _factoryOnCurrentLevel.CreateConstruction<MilitaryOffice>(transform.position);
                break;

            }
        }

        private void CreateAndInitializeFactory(BordersSpawnTransform bordersSpawnTransform)
        {
            _factoryOnCurrentLevel = new GameFactory
                .GameFactory(_staticDataService, _coroutineRunner ,bordersSpawnTransform, _playerProgressServiceOnCurrentLevel, _soundDataService);
        }

        private void CreatePlayerProgress()
        {
            _playerProgressServiceOnCurrentLevel = new PlayerProgressService(_staticDataService);
        }

        private void CreateHUD()
        {
            _factoryOnCurrentLevel.CreateHUD();
        }
    }
}