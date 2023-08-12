using System.Collections.Generic;
using Code.Infrastructure.Services.PlayerProgressService;
using Code.Logic.Buildings;
using Code.Logic.Monster;
using Code.Logic.Monster.Eggs;
using UnityEngine;

namespace Code.Infrastructure.GameFactory
{
    public interface IGameFactory
    {
        List<MonsterEgg> ActiveEggs { get; }
        List<MonsterBattleStrength> ActiveMonsters { get; }

        MonsterEgg CreateEgg(Transform parent);
        GameObject CreateRandomMonsterWithCheapestRareLevel(Vector3 position);

        GameObject CreateMonsterWithBattleStrengthValueFromMonsters(Vector3 position,
            IEnumerable<MonsterStateMachine> monsters);

        GameObject[] CreateCheapestMonstersFromMonstersBattleStrengthValue(Vector3 spawnPointPosition, IEnumerable<MonsterStateMachine> monsters);
        
        void RemoveEgg(MonsterEgg egg);
        void RemoveMonster(MonsterBattleStrength monster);

        GameObject CreateConstruction<T>(Vector3 position) where T : Construction;
        void CreateHUD();
    }
}