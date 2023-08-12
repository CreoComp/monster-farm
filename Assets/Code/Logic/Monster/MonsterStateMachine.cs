using System;
using System.Collections.Generic;
using Code.Infrastructure;
using Code.Logic.Buildings;
using Code.Logic.Monster.MonsterStates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace Code.Logic.Monster
{
    public class MonsterStateMachine : MonoBehaviour
    {
        [FormerlySerializedAs("_layorMaskWithoutMonster"), FormerlySerializedAs("maskWithoutMonster")] [SerializeField]
        private LayerMask _layerMaskWithoutMonster;

        private BordersSpawnTransform _bordersSpawnTransform;

        [FormerlySerializedAs("pointOfJump"), HideInInspector]
        public Vector3 PointOfJump;

        private Rigidbody _rigidbody;

        private Dictionary<Type, IMonsterState> _states;
        public IMonsterState CurrentState { get; private set; }

        private ICoroutineRunner _coroutineRunner;

        private SoundDataService _soundDataService;
        
        public MonsterPlatform CurrentPlatform { get; set; }


        public void Construct(BordersSpawnTransform bordersSpawnTransform, ICoroutineRunner coroutineRunner, SoundDataService soundDataService)
        {
            _bordersSpawnTransform = bordersSpawnTransform;
            _coroutineRunner = coroutineRunner;
            _soundDataService = soundDataService;
            Debug.Log($"<color=red> :{_soundDataService} </color> ");

            RegisterStates();
            
            _rigidbody = GetComponent<Rigidbody>();
            EnterIn<ChaoticWalkState>();
        }

        public void EnterIn<TState>() where TState : IMonsterState
        {
            if (_states.TryGetValue(typeof(TState), out IMonsterState state))
            {
                Debug.Log($"{name} entered state {state.GetType()}");
                CurrentState?.Exit();
                CurrentState = state;
                CurrentState.Enter();
            }
        }

        private void Update()
        {
            CurrentState.Update();
        }

        private void FixedUpdate()
        {
            if (CurrentState is IFixedUpdate fixedUpdate)
                fixedUpdate.FixedUpdate();
        }

        public void AddForceOutOfConstruction(Vector3 posConstruction)
        {
            PointOfJump = posConstruction;
            EnterIn<JumpingState>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (CurrentState is ICollisionState collisionState)
                collisionState.OnCollisionEnter(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (CurrentState is ICollisionState collisionState)
                collisionState.OnCollisionEnter(collision);
        }

        private void RegisterStates()
        {
            _states = new Dictionary<Type, IMonsterState>()
            {
                [typeof(IdleState)] = new IdleState(this, GetComponentInChildren<Animator>()),
                [typeof(HatchingEggState)] = new HatchingEggState(this),
                [typeof(DragAndDropState)] = new DragAndDropState(this, Camera.main, _layerMaskWithoutMonster),
                [typeof(ChaoticWalkState)] = new ChaoticWalkState(this, GetComponent<NavMeshAgent>(),
                    _bordersSpawnTransform, GetComponentInChildren<Animator>()),
                [typeof(JumpingState)] = new JumpingState(this, GetComponentInChildren<Animator>(),
                    GetComponent<Rigidbody>(), _coroutineRunner, _soundDataService),
            };
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if(CurrentState is ITriggerState triggerState)
        //         triggerState.OnTriggerEnter(other);
        // }
        //
        // private void OnTriggerStay(Collider other)
        // {
        //     if (CurrentState is ITriggerState triggerState)
        //         triggerState.OnTriggerEnter(other);
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     if (CurrentState is ITriggerState triggerState)
        //         triggerState.OnTriggerExit(other);
        // }
    }
}