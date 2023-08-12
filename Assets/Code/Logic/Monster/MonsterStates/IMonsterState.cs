namespace Code.Logic.Monster.MonsterStates
{
    public interface IMonsterState
    {
        public void Enter();
        public void Exit();
        public void Update();
    }
}