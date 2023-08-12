using System;
using System.Collections.Generic;
using Code.Logic.Buildings;
using Code.Logic.Monster.Eggs;
using Code.Logic.Monster.MonsterData;

namespace Code.Infrastructure.Services.StaticDataService
{
    public interface IStaticDataService : IService
    {
        MonsterEgg EggPrefab { get; }
        List<KeyValuePair<MonsterRarityLevel, List<MonsterData>>> MonsterDataGroupedByRarityLevel { get; }
        Dictionary<Type, Construction> ConstructionPrefabsGroupedByType { get; }

        void Load();
    }
}