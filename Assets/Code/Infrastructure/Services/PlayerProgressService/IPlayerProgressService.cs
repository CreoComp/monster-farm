using System;
using System.Collections.Generic;
using Code.Logic.Monster.MonsterData;

namespace Code.Infrastructure.Services.PlayerProgressService
{
    public interface IPlayerProgressService
    {
        List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>> LockedMonstersGroupedByRarityLevel { get; }
        List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>> UnlockedMonstersGroupedByRarityLevel { get; }
        
        int MoneyAmount { get; }
        Action MoneyAmountChanged { get; set; }
        Action MonsterUnlocked { get; set; }
        void UnlockMonster(MonsterData monster, int rarityLevelIndex);
        void AddMoney(int amount);
    }
}