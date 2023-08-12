using System.Collections;
using Code.Infrastructure.GameFactory;
using Code.Logic.Monster.MonsterStates;
using UnityEngine;
using Zenject;

namespace Code.Logic.Monster.Eggs
{
    public class MonsterEgg : MonoBehaviour
    {
        private IGameFactory _gameFactory;
        private SoundDataService _soundDataService;

        [Inject]
        public void Constructor(SoundDataService soundDataService)
        {
            _soundDataService = soundDataService;
        }

        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            StartCoroutine(HatchAfterDelayCoroutine(3f));
        }

        private IEnumerator HatchAfterDelayCoroutine(float delayInSecond)
        {
            yield return new WaitForSeconds(delayInSecond);
            _soundDataService.OpenEgg();

            GameObject randomOrdinaryMonster =
                _gameFactory.CreateRandomMonsterWithCheapestRareLevel(transform.position);
            randomOrdinaryMonster.GetComponent<MonsterStateMachine>().EnterIn<JumpingState>();
            _gameFactory.RemoveEgg(this);
        }
    }
}