using System;
using System.Collections.Generic;
using Code.Infrastructure.Services.StaticDataService;
using Code.Logic.Monster.MonsterData;
using UnityEngine;

namespace Code.Infrastructure.Services.PlayerProgressService
{
    class PlayerProgressService : IPlayerProgressService
    {
        public List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>> LockedMonstersGroupedByRarityLevel { get; private set; }
        public List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>> UnlockedMonstersGroupedByRarityLevel { get; private set; }
        public Action MonsterUnlocked { get; set; }
        
        public int MoneyAmount { get; private set; }
        public Action MoneyAmountChanged { get; set; }

        public PlayerProgressService(IStaticDataService staticDataService)
        {
            InitializeLockedMonsters(staticDataService);
        }

        public void UnlockMonster(MonsterData monster, int rarityLevelIndex)
        {
            if (!LockedMonstersGroupedByRarityLevel[rarityLevelIndex].Value.Remove(monster))
            {
                Debug.LogWarning($"{monster} already unlocked or did not added to list while initializing");
                return;
            }

            if (!UnlockedMonstersGroupedByRarityLevel[rarityLevelIndex].Value.Contains(monster))
            {
                UnlockedMonstersGroupedByRarityLevel[rarityLevelIndex].Value.Add(monster);
                Debug.Log($"Unlocked new monster - {monster.name}");
                Debug.Log($"{LockedMonstersGroupedByRarityLevel[rarityLevelIndex].Value.Count} monsters left locked from {LockedMonstersGroupedByRarityLevel[rarityLevelIndex].Key.name}");
                MonsterUnlocked?.Invoke();
            }
            else
            {
                Debug.Log($"{monster} already contains in list of unlocked monsters");
            }
        }

        public void AddMoney(int amount)
        {
            if (amount < 0)
            {
                Debug.Log("Invalid money amount");
                return;
            }

            MoneyAmount += amount;
            MoneyAmountChanged?.Invoke();
        }

        private void InitializeLockedMonsters(IStaticDataService staticDataService)
        {
            LockedMonstersGroupedByRarityLevel = staticDataService.MonsterDataGroupedByRarityLevel;
            UnlockedMonstersGroupedByRarityLevel = new List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>>();
            foreach (var keyValuePair in staticDataService.MonsterDataGroupedByRarityLevel)
            {
                UnlockedMonstersGroupedByRarityLevel
                    .Add(new KeyValuePair<MonsterRarityLevel, List<MonsterData>>(keyValuePair.Key, new List<MonsterData>()));
            }
        }
    }
}