using Code.Infrastructure.Services.StaticDataService;

namespace Code.Infrastructure.GameStates
{
    public class BootstrapState : IGameState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;

        public BootstrapState(GameStateMachine stateMachine, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _staticDataService.Load();
            _stateMachine.EnterState<LoadLevelState>();
        }

        public void Exit()
        {
            
        }
    }
}