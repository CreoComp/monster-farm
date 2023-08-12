namespace Code.Logic.Monster.MonsterStates
{
    public class HatchingEggState : IMonsterState
    {
        private readonly MonsterStateMachine _monsterStateMachine;

        public HatchingEggState(MonsterStateMachine monsterStateMachine)
        {
            _monsterStateMachine = monsterStateMachine;
        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}