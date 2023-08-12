using System.Collections;
using System.Collections.Generic;
using Code.Infrastructure.GameFactory;
using Code.Infrastructure.Services.PlayerProgressService;
using Code.Logic.Monster;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Code.Logic.Buildings
{
    public class ConnectDivideMonsters : Construction
    {
        [SerializeField] private float _connectDivideMonsterTime = 3f;

        [FormerlySerializedAs("CanvasButtons"), SerializeField]

        private List<MonsterStateMachine> _monsterStateMachines;

        
        public override void Construct(IGameFactory gameFactory, IPlayerProgressService playerProgressService ,SoundDataService soundDataService)
        {
            base.Construct(gameFactory, playerProgressService, soundDataService);
        }

        public void ConnectMonsters()
        {

            if (_monsterStateMachines == null ) 
                return;

            if (_monsterStateMachines.Count == 1)
                return;

            SoundDataService.ConnectDivideMonsters();
            StartCoroutine(SpawnMonstersAfterConnectTimeCoroutine());
        }

        public void DivideMonsters()
        {
            if (_monsterStateMachines == null)
                return;
            


            SoundDataService.ConnectDivideMonsters();
            StartCoroutine(SplitMonstersToCheaper());
        }

        private IEnumerator SpawnMonstersAfterConnectTimeCoroutine()
        {
            // yield return new WaitForSeconds(ConnectDivideMonsterTime);
            GameFactory.CreateMonsterWithBattleStrengthValueFromMonsters(SpawnPoint.position, _monsterStateMachines);
            RemoveMonsters();
            yield return null;
        }

        private IEnumerator SplitMonstersToCheaper()
        {
            // yield return new WaitForSeconds(ConnectDivideMonsterTime);
            GameFactory.CreateCheapestMonstersFromMonstersBattleStrengthValue(SpawnPoint.position, _monsterStateMachines);
            RemoveMonsters();
            yield return null;
        }

        private void RemoveMonsters()
        {
            foreach (var monster in _monsterStateMachines)
            {
                GameFactory.RemoveMonster(monster.GetComponent<MonsterBattleStrength>());
            }

            Platform.ClearCurrentMonstersList();
        }

        protected override void OnMonsterCountUpdate(List<MonsterStateMachine> monsterStateMachines)
        {
            _monsterStateMachines = monsterStateMachines;
        }

        protected override void OnConstructionIsFull(List<MonsterStateMachine> monsterStateMachines)
        {

        }

        public override void InvokeDestinedAction()
        {

        }


        /*private IEnumerator IncubationTimeLine()
        {
          //  yield return new WaitForSeconds(IncubationMonsterTime);
          //  _gameFactory.CreateEgg(SpawnPoint);

         //   ReleaseMonster();
          //  Platform.ClearCurrentMonstersList();
        }*/
    }
}