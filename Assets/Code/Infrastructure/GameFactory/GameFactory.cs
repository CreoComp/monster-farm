using System;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Services.PlayerProgressService;
using Code.Infrastructure.Services.StaticDataService;
using Code.Logic;
using Code.Logic.Buildings;
using Code.Logic.Monster;
using Code.Logic.Monster.Eggs;
using Code.Logic.Monster.MonsterData;
using Code.UI;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Infrastructure.GameFactory
{
    public class GameFactory : IGameFactory
    {
        private const int ChanceToUnlockNewMonster = 70;
        public List<MonsterEgg> ActiveEggs { get; } = new();
        public List<MonsterBattleStrength> ActiveMonsters { get; } = new();
        
        private readonly MonsterEgg _eggPrefab;
        private readonly Dictionary<Type, Construction> _constructionsPrefabs;

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly BordersSpawnTransform _bordersSpawnTransform;
        private readonly IPlayerProgressService _playerProgress;
        private SoundDataService _soundDataService;



        public GameFactory(IStaticDataService dataService, ICoroutineRunner coroutineRunner,BordersSpawnTransform bordersSpawnTransform, IPlayerProgressService playerProgress, SoundDataService soundDataService)
        {
            _playerProgress = playerProgress;
            _eggPrefab = dataService.EggPrefab;
            _constructionsPrefabs = dataService.ConstructionPrefabsGroupedByType;

            _coroutineRunner = coroutineRunner;
            _bordersSpawnTransform = bordersSpawnTransform;
            _soundDataService  = soundDataService;
        }
        
        public MonsterEgg CreateEgg(Transform parent)
        {
            MonsterEgg monsterEgg = Object.Instantiate(_eggPrefab, parent);
            monsterEgg.Construct(this);

            ActiveEggs.Add(monsterEgg);

            Debug.Log("Created new egg.");
            
            return monsterEgg;
        }

        public GameObject CreateRandomMonsterWithCheapestRareLevel(Vector3 position)
        {
            var monsterData = ChoseRandomMonsterFromRareCategoryByIndex(0);
            return CreateMonster(position, monsterData);
        }

        public GameObject CreateMonsterWithBattleStrengthValueFromMonsters(Vector3 position,
            IEnumerable<MonsterStateMachine> monsters)
        {
            int value = GetBattleStrengthValueSumFromMonsters(monsters);
            if (value < _playerProgress.LockedMonstersGroupedByRarityLevel[0].Key.MinBattleStrengthValue)
            {
                Debug.LogWarning("Taken less battle strength value than minimum value from cheapest rare level");
                return null;
            }

            var index = CalculateRareLevelIndex(value);;
            var monsterData = ChoseRandomMonsterFromRareCategoryByIndex(index);
            
            return CreateMonster(position, monsterData);
        }

        public GameObject[] CreateCheapestMonstersFromMonstersBattleStrengthValue
            (Vector3 spawnPointPosition, IEnumerable<MonsterStateMachine> monsters)
        {
            int value = GetBattleStrengthValueSumFromMonsters(monsters);
            int monstersAmount = value / _playerProgress.LockedMonstersGroupedByRarityLevel[0].Key.MinBattleStrengthValue;

            for (int i = 0; i < monstersAmount; i++)
            {
                CreateRandomMonsterWithCheapestRareLevel(spawnPointPosition);
            }
            
            return null;
        }

        public void RemoveEgg(MonsterEgg egg)
        {
            ActiveEggs.Remove(egg);
            Object.Destroy(egg.gameObject);
            
            Debug.Log("Egg removed");
        }

        public void RemoveMonster(MonsterBattleStrength monster)
        {
            ActiveMonsters.Remove(monster);
            Object.Destroy(monster.gameObject);
            
            Debug.Log("Monster removed");
        }

        public GameObject CreateConstruction<T>(Vector3 position) where T : Construction
        {
            if (_constructionsPrefabs.TryGetValue(typeof(T), out var construction))
            {
                GameObject constructionGameObject = Object.Instantiate(construction.gameObject, position, Quaternion.identity);
                constructionGameObject.GetComponent<Construction>().Construct(this, _playerProgress, _soundDataService);
                return constructionGameObject;
            }
            
            Debug.LogWarning($"Did not find prefab of type {typeof(T)} to spawn");
            return null;
        }

        public void CreateHUD()
        {
            var progressCounterPrefab = Resources.Load<Canvas>("Prefabs/UI/HUD");
            var hud = Object.Instantiate(progressCounterPrefab);
            
            var buttons = hud.GetComponentsInChildren<BuildConstructionsButton>();
            foreach(BuildConstructionsButton button in buttons)
            {
                button.Construct(this, _playerProgress);
            }
            hud.GetComponentInChildren<UnlockedMonstersProgressUI>().Construct(_playerProgress, _soundDataService);
            hud.GetComponentInChildren<MoneyCounterUI>().Construct(_playerProgress);
        }

        private GameObject CreateMonster(Vector3 position, MonsterData monsterData)
        {
            GameObject monster = Object.Instantiate(monsterData.Prefab, position, Quaternion.identity);


            monster.GetComponent<MonsterStateMachine>().Construct(_bordersSpawnTransform, _coroutineRunner, _soundDataService);
            
            var monsterBattleStrength = monster.GetComponent<MonsterBattleStrength>();
            monsterBattleStrength
                .Construct(monsterData.RarityLevel.MinBattleStrengthValue, monsterData.RarityLevel.MaxBattleStrengthValue);
            
            ActiveMonsters.Add(monsterBattleStrength);
            Debug.Log($"Spawned monster with: \nName - {monsterData.name}, \nRarity level - {monsterData.RarityLevel.LevelName}");
            var outline = monster.gameObject.GetComponent<QuickOutline.Scripts.Outline>();
            outline.OutlineColor = monsterData.RarityLevel.OutlineColor;
            return monster;
        }

        private static int GetBattleStrengthValueSumFromMonsters(IEnumerable<MonsterStateMachine> monsters)
        {
            return monsters.Sum(monster => monster.GetComponent<MonsterBattleStrength>().BattleStrengthValue);
        }

        private MonsterData ChoseRandomMonsterFromRareCategoryByIndex(int index)
        {
            int randomChance = Random.Range(0, 100);
            var lockedRarityGroup = _playerProgress.LockedMonstersGroupedByRarityLevel[index].Value;
            var unlockedRarityGroup = _playerProgress.UnlockedMonstersGroupedByRarityLevel[index].Value;
            MonsterData chosenMonster;
            if (randomChance <= ChanceToUnlockNewMonster && lockedRarityGroup.Count > 0)
            {
                chosenMonster = UnlockMonster(index);
            }
            else
            {
                chosenMonster = unlockedRarityGroup.Count > 0 
                    ? unlockedRarityGroup[Random.Range(0, unlockedRarityGroup.Count)] 
                    : UnlockMonster(index);
            }

            return chosenMonster;
        }

        private int CalculateRareLevelIndex(int inputBattleStrengthValue)
        {
            int index = 0;
            bool timeToChose = false;
            while (!timeToChose)
            {
                int count = _playerProgress.LockedMonstersGroupedByRarityLevel.Count;
                if (index < count)
                {
                    if (index + 1 < count && inputBattleStrengthValue >=
                        _playerProgress.LockedMonstersGroupedByRarityLevel[index + 1].Key.MinBattleStrengthValue)
                    {
                        index++;
                        continue;
                    }

                    timeToChose = true;
                }
            }

            return index;
        }

        private MonsterData UnlockMonster(int lockedIndexRarityGroup)
        {
            var lockedMonsters = _playerProgress.LockedMonstersGroupedByRarityLevel[lockedIndexRarityGroup].Value;
            var chosenMonster = lockedMonsters[Random.Range(0, lockedMonsters.Count)];
            _playerProgress.UnlockMonster(chosenMonster, lockedIndexRarityGroup);
            return chosenMonster;
        }
    }
}