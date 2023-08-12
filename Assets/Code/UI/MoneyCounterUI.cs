using Code.Infrastructure.Services.PlayerProgressService;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class MoneyCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private IPlayerProgressService _playerProgress;

        public void Construct(IPlayerProgressService playerProgressService)
        {
            _playerProgress = playerProgressService;
            _playerProgress.MoneyAmountChanged += MoneyAmountChanged;
            MoneyAmountChanged();
        }

        private void OnDestroy()
        {
            if (_playerProgress != null)
            {
                _playerProgress.MoneyAmountChanged -= MoneyAmountChanged;
            }
        }

        private void MoneyAmountChanged()
        {
            _text.text = _playerProgress.MoneyAmount.ToString();
        }
    }
}