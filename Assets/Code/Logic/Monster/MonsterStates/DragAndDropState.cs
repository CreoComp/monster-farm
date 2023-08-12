using Code.Logic.Buildings;
using UnityEngine;

namespace Code.Logic.Monster.MonsterStates
{
    public class DragAndDropState : IMonsterState
    {
        private readonly MonsterStateMachine _monsterStateMachine;
        private readonly Camera _camera;
        private readonly LayerMask _maskWithoutMonster;

        private MonsterPlatform _platform;
        private Collider _collider;

        private float _yOffset = 1.5f;
        private Ray _ray;
        private RaycastHit _raycastHit;

        public DragAndDropState(MonsterStateMachine monsterStateMachine, Camera camera, LayerMask maskWithoutMonster)
        {
            _monsterStateMachine = monsterStateMachine;
            _camera = camera;
            _maskWithoutMonster = maskWithoutMonster;
        }

        public void Enter()
        {
            _collider = _monsterStateMachine.GetComponent<Collider>();
            _collider.enabled = false;
            if (_platform != null)
            {
                _platform.RemoveFromCurrent(_monsterStateMachine);
                _platform = null;
            }
        }

        public void Exit()
        {
            _collider.enabled = true;
        }

        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_platform != null)
                {
                    if (!_platform.IsPlatformFull())
                    {
                        _platform.AddToCurrent(_monsterStateMachine);
                    }
                    else
                    {
                        _monsterStateMachine.EnterIn<JumpingState>();
                    }
                }
                else
                {
                    _monsterStateMachine.EnterIn<ChaoticWalkState>();
                }
            }

            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _raycastHit, _maskWithoutMonster))
            {
                var platform = _raycastHit.transform.gameObject.GetComponent<MonsterPlatform>();

                if (platform != null)
                {
                    _platform = platform;
                }
                else
                {
                    _platform = null;
                }

                Vector3 pointToMove = new Vector3(_raycastHit.point.x, _raycastHit.point.y + _yOffset,
                    _raycastHit.point.z);
                _monsterStateMachine.transform.position = pointToMove;
            }
        }
    }
}