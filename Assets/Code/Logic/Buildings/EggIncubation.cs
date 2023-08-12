using System.Collections;
using System.Collections.Generic;
using Code.Infrastructure.GameFactory;
using Code.Logic.Monster;
using Code.Logic.Monster.MonsterStates;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Code.Logic.Buildings
{
    public class EggIncubation : Construction
    {
        [FormerlySerializedAs("IncubationMonsterTime"), SerializeField]
        private float _hatchingMonsterTime = 3f;

        private bool _isMoveMonster;
        private bool _isHatching;
        private const float ToleranceDistanceToMoveMonster = 0.3f;

        private List<MonsterStateMachine> _monsterStateMachine;
        private Coroutine _coroutineInstance;

        protected override void Update()
        {
            if (_isMoveMonster)
            {
                MoveMonsterToSpawnPoint();
            }

            if(_isHatching)
            HatchingMonster();

            base.Update();
        }

        public void CancelIncubation()
        {
            StopCoroutine(_coroutineInstance);
            Platform.ClearCurrentMonstersList();
        }

        public override void InvokeDestinedAction()
        {
        }

        protected override void OnConstructionIsFull(List<MonsterStateMachine> monsterStateMachines)
        {
            _isMoveMonster = true;
            _monsterStateMachine = monsterStateMachines;
        }

        protected override void OnMonsterCountUpdate(List<MonsterStateMachine> monsterStateMachines)
        {
        }

        private void MoveMonsterToSpawnPoint()
        {
            if (_monsterStateMachine.Count < 0)
            {
                return;
            }

            Transform monster = _monsterStateMachine[0].transform;
            Quaternion rotationAngle = Quaternion.Euler(0, 180, 0);
            float speedMove = 4f * UnityEngine.Time.deltaTime;
            float distance = Vector3.Distance(monster.position, SpawnPoint.position);

            if (distance > ToleranceDistanceToMoveMonster)
            {
                monster.GetComponent<Rigidbody>().useGravity = false;
                monster.position = Vector3.Lerp(_monsterStateMachine[0].transform.position, SpawnPoint.position,
                    speedMove);

                float time = distance / speedMove;
                monster.rotation = Quaternion.Lerp(monster.rotation, rotationAngle, time);
            }
            else
            {
                monster.GetComponent<Rigidbody>().useGravity = true;
                monster.LookAt(Vector3.back * 20f);

                _isMoveMonster = false;
                _isHatching = true;
                Time = _hatchingMonsterTime;
            }
        }

        private void ReleaseMonster()
        {
            _monsterStateMachine[0].EnterIn<JumpingState>();
        }

        private void HatchingMonster()
        {

            Time -= UnityEngine.Time.deltaTime;

            if (Time <= 0 )
            {
                Time = 0;
                _isHatching = false;
                if (_monsterStateMachine.Count > 0 && _monsterStateMachine[0].CurrentState.GetType() == typeof(IdleState))
                {
                    GameFactory.CreateEgg(SpawnPoint);
                    ReleaseMonster();
                    Platform.SetEggIntoPlatform(true);
                }

                Platform.ClearCurrentMonstersList();
            }

        }
    }
}