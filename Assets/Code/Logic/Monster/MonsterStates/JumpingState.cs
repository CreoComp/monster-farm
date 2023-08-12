using System.Collections;
using Code.Infrastructure;
using Code.Logic.Markers;
using UnityEngine;

namespace Code.Logic.Monster.MonsterStates
{
    public class JumpingState : IMonsterState, ICollisionState, IFixedUpdate
    {
        private const float ForceJump = 1500f;
        private const float TimeJumpUp = .3f;

        public Vector3 PointOfJump;
        private float _timeInJump;
        private float _time;
        private bool _isJumpingUp;

        private readonly Transform _monster;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Animator _animator;
        private readonly Rigidbody _rigidbody;
        private readonly MonsterStateMachine _monsterStateMachine;
        private readonly SoundDataService _soundDataService;

        public JumpingState(MonsterStateMachine monsterStateMachine, Animator animator, Rigidbody rigidbody,
            ICoroutineRunner coroutineRunner, SoundDataService soundDataService)
        {
            _monsterStateMachine = monsterStateMachine;
            _animator = animator;
            _rigidbody = rigidbody;
            _coroutineRunner = coroutineRunner;

            _monster = _monsterStateMachine.transform;
            _soundDataService = soundDataService;
        }

        public void Enter()
        {
            _soundDataService.JumpMonster();
            _monsterStateMachine.transform.eulerAngles = new Vector3(0, 180, 0);
            _timeInJump = TimeInJump();
            _monsterStateMachine.GetComponent<Collider>().enabled = true;
            PointOfJump = _monsterStateMachine.PointOfJump;
            _rigidbody.useGravity = true;
            _animator.SetBool("Jump", true);
            _coroutineRunner.StartCoroutine(JumpCoroutine());
        }

        public void Exit()
        {
            /*_monsterStateMachine.GetComponent<Collider>().enabled = false;*/
            _animator.SetBool("Jump", false);
            _rigidbody.useGravity = false;
        }

        public void Update()
        {

        }


        public void OnCollisionEnter(Collision collision)
        {
            if (!_isJumpingUp)
            {
                if (collision.gameObject.GetComponent<GroundMarker>())
                {
                    _monsterStateMachine.EnterIn<ChaoticWalkState>();
                }
            }
        }

        public void FixedUpdate()
        {
            FlipInJump();
        }

        private IEnumerator JumpCoroutine()
        {
            _isJumpingUp = true;
            _rigidbody.AddForce(Vector3.up * ForceJump, ForceMode.Impulse);
            yield return new WaitForSeconds(TimeJumpUp);
            _isJumpingUp = false;

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.forward * ForceJump, ForceMode.Impulse);
        }

        private void FlipInJump()
        {
            _time += Time.fixedDeltaTime;
            float callsFlip = (_timeInJump / Time.fixedDeltaTime);

            _monster.Rotate(new Vector3(-(360f / callsFlip), 0, 0));
            //_monster.rotation = Quaternion.FromToRotation(_monster.transform.eulerAngles, );
            //Debug.Log(time);
        }

        private float TimeInJump()
        {
            float mass = _rigidbody.mass;
            float distanceJumpUp =
                (ForceJump * TimeJumpUp) + ((-9.81f * mass * TimeJumpUp * TimeJumpUp) / 2f); // S = Vt + (gtt / 2)
            float timeFallDown = distanceJumpUp / (9.81f * mass); // t = s / v
            float timeInJump = TimeJumpUp + timeFallDown;

            return 1.31f;
        }
    }
}