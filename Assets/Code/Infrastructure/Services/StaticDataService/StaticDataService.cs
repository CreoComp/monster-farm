using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Code.Logic.Buildings;
using Code.Logic.Monster.Eggs;
using Code.Logic.Monster.MonsterData;
using UnityEngine;

namespace Code.Infrastructure.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        public MonsterEgg EggPrefab { get; private set; }
        public List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>> MonsterDataGroupedByRarityLevel 
        { get; private set; }
        public Dictionary<Type, Construction> ConstructionPrefabsGroupedByType { get; private set; }

        public void Load()
        {
            LoadMonstersData();
            LoadEggPrefab();
            LoadConstructionPrefabs();
        }

        private void LoadEggPrefab()
        {
            EggPrefab = Resources.Load<MonsterEgg>("Prefabs/Egg/Egg");
            Debug.Log("Resources loaded: Egg prefab");
        }

        private void LoadMonstersData()
        {
            MonsterData[] allMonsters = Resources.LoadAll<MonsterData>("StaticData/MonsterData");
            var monstersData = allMonsters.GroupBy(monsterData => monsterData.RarityLevel);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Resources loaded: Monsters with \nRarity Level:");

            MonsterDataGroupedByRarityLevel = new List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>>();
            foreach (IGrouping<MonsterRarityLevel, MonsterData> dataServiceMonster in monstersData)
            {
                MonsterRarityLevel key = dataServiceMonster.Key;
                List<MonsterData> selectedGroup = dataServiceMonster.Select(m => m).ToList();
                MonsterDataGroupedByRarityLevel
                    .Add(new KeyValuePair<MonsterRarityLevel, List<MonsterData>>(key, selectedGroup));

                stringBuilder.Append($"\n {key.LevelName} - {selectedGroup.Count.ToString()} items");
            }

            MonsterDataGroupedByRarityLevel = MonsterDataGroupedByRarityLevel
                .OrderBy(pair => pair.Key.MinBattleStrengthValue).ToList();
            
            Debug.Log(stringBuilder.ToString());
        }

        private void LoadConstructionPrefabs()
        {
            ConstructionPrefabsGroupedByType = Resources.LoadAll<Construction>("Prefabs/Constructions")
                .ToDictionary(construction => construction.GetComponent<Construction>().GetType(),
                    construction => construction);
        }
    }
}