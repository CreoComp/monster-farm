using Code.Logic.Buildings;
using UnityEngine;

namespace Code.Logic.Monster.MonsterStates
{
    public class IdleState : IMonsterState
    {
        private readonly Animator _animator;
        private readonly MonsterStateMachine _monsterStateMachine;

        private MonsterPlatform _currentPlatform;

        public IdleState(MonsterStateMachine monsterStateMachine, Animator animator)
        {
            _monsterStateMachine = monsterStateMachine;
            _animator = animator;
        }

        public void Enter()
        {
            _currentPlatform = _monsterStateMachine.CurrentPlatform;
            _monsterStateMachine.GetComponent<Collider>().enabled = true;
            _monsterStateMachine.GetComponent<Rigidbody>().useGravity = true;
            _animator.SetBool("Walk", false);
        }

        public void Exit()
        {
            _monsterStateMachine.GetComponent<Collider>().enabled = false;
            _monsterStateMachine.GetComponent<Rigidbody>().useGravity = false;

            _animator.SetBool("Walk", true);

            if (_currentPlatform == null)
            {
                return;
            }
            _currentPlatform.RemoveFromCurrent(_monsterStateMachine);
            _currentPlatform = null;
        }

        public void Update()
        {

        }
    }
}