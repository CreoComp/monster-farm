using System.Collections;
using System.Linq;
using Code.Infrastructure.Services.PlayerProgressService;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.UI
{
    public class UnlockedMonstersProgressUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counter;

        [SerializeField] private TMP_Text  _onUnlockedMonsterPopup;
        [SerializeField] private float _timeForShowingPopupInSeconds = 2f;
        
        private IPlayerProgressService _playerProgress;
        private Coroutine _coroutine;

        private SoundDataService _soundDataService;


        public void Construct(IPlayerProgressService playerProgressService, SoundDataService soundDataService)
        {
            _playerProgress = playerProgressService;
            _soundDataService = soundDataService;

            _onUnlockedMonsterPopup.enabled = false;
            _playerProgress.MonsterUnlocked += OnMonsterUnlocked;
        }

        private void OnDestroy()
        {
            Debug.Log("Destroy");
            if (_playerProgress != null)
            {
                _playerProgress.MonsterUnlocked -= OnMonsterUnlocked;
            }
        }

        private void OnMonsterUnlocked()
        {
            _soundDataService.UnlockNewMonster();
            var unlockedMonstersCount = _playerProgress.UnlockedMonstersGroupedByRarityLevel.SelectMany(p => p.Value).Count();
            var lockedMonstersCount = _playerProgress.LockedMonstersGroupedByRarityLevel.SelectMany(p => p.Value).Count();
            _counter.text = $"{unlockedMonstersCount.ToString()}/{(unlockedMonstersCount + lockedMonstersCount).ToString()}";

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(ShowPopupForTimeInSecondsCoroutine(_timeForShowingPopupInSeconds));
        }

        private IEnumerator ShowPopupForTimeInSecondsCoroutine(float time)
        {
            _onUnlockedMonsterPopup.enabled = true;
            yield return new WaitForSeconds(time);
            _onUnlockedMonsterPopup.enabled = false;
        }
    }
}