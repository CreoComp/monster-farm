using Code.Logic.Monster.MonsterStates;
using UnityEngine;
using Zenject;

namespace Code.Logic.Monster
{
    public class MonsterToDragSelector : ITickable, IInitializable
    {
        private Camera _camera;

        private Ray _ray;
        private RaycastHit _raycastHit;

        private MonsterStateMachine _currentStateMachine;

        public void Initialize()
        {
            _camera = Camera.main;
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _raycastHit))
                {
                    if (_raycastHit.transform.TryGetComponent(out _currentStateMachine))
                    {
                        Debug.Log($"{_currentStateMachine.name} selected as currently dragged");

                        if (_currentStateMachine.CurrentState is IdleState)
                        {
                            Debug.Log("<color=red> idleState </color>");

                            _currentStateMachine.EnterIn<JumpingState>();
                            return;

                        }
                        _currentStateMachine.EnterIn<DragAndDropState>();
                    }
                }
            }
        }
    }
}