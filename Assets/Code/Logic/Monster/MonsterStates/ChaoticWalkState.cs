using UnityEngine;
using UnityEngine.AI;

namespace Code.Logic.Monster.MonsterStates
{
    public class ChaoticWalkState : IMonsterState
    {
        private readonly NavMeshAgent _agent;
        private readonly BordersSpawnTransform _bordersSpawnTransform;
        private readonly Animator _animator;
        private readonly MonsterStateMachine _monsterStateMachine;

        private Vector3 _positionToMove;

        public ChaoticWalkState(MonsterStateMachine monsterStateMachine, NavMeshAgent agent,
            BordersSpawnTransform bordersSpawnTransform, Animator animator)
        {
            _monsterStateMachine = monsterStateMachine;
            _agent = agent;
            _bordersSpawnTransform = bordersSpawnTransform;
            _animator = animator;
        }

        public void Enter()
        {
            _agent.enabled = true;
            _animator.SetBool("Walk", true);
            _agent.isStopped = false;
            SetMovePosition();
        }

        public void Exit()
        {
            _animator.SetBool("Walk", false);
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        public void Update()
        {
            if (Vector3.Distance(_monsterStateMachine.transform.position, _positionToMove) <= _agent.stoppingDistance)
            {
                SetMovePosition();
            }
        }

        private void SetMovePosition()
        {
            _positionToMove = _bordersSpawnTransform.GetRandomPositionInLocation();

            _agent.SetDestination(_positionToMove);
        }
    }
}