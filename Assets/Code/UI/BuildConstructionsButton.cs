using System;
using Code.Infrastructure.GameFactory;
using Code.Infrastructure.Services.PlayerProgressService;
using Code.Logic.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class BuildConstructionsButton : MonoBehaviour
    {
        private const string IncubatorSpawnPointTag = "IncubatorSpawnPoint";
        private const string ConnectorSpawnPointTag = "ConnectorSpawnPoint";
        private const string MilitaryOfficeSpawnPoint = "MilitaryOfficeSpawnPoint";

        [SerializeField] private TextMeshProUGUI _textCost;
        [SerializeField] private int _costUpdate = 500;
        [SerializeField] private Constructions _constructionType;

        private GameObject[] _spawnPlaces;

        private int _nowConstructions;

        private IGameFactory _gameFactory;
        private IPlayerProgressService _playerProgress;
        private Button _button;

        public void Construct(IGameFactory gameFactory, IPlayerProgressService playerProgressService)
        {
            _gameFactory = gameFactory;
            _playerProgress = playerProgressService;
            _playerProgress.MoneyAmountChanged += UpdateButton;
        }

        private void Start()
        {
            SetSpawnPoints();
            _button = GetComponent<Button>();
            _textCost.text = _nowConstructions.ToString();
        }

        private void OnDestroy()
        {
            _playerProgress.MoneyAmountChanged -= UpdateButton;
        }

        private void SetSpawnPoints()
        {
            if (_constructionType == Constructions.Incubator)
                _spawnPlaces = GameObject.FindGameObjectsWithTag(IncubatorSpawnPointTag);

            else if (_constructionType == Constructions.Connector)
                _spawnPlaces = GameObject.FindGameObjectsWithTag(ConnectorSpawnPointTag);

            else if (_constructionType == Constructions.MilitaryOffice)
                _spawnPlaces = GameObject.FindGameObjectsWithTag(MilitaryOfficeSpawnPoint);
        }

        public void Buy()
        {
            if (_constructionType == Constructions.Incubator)
                _gameFactory.CreateConstruction<EggIncubation>(_spawnPlaces[_nowConstructions].transform.position);

            else if (_constructionType == Constructions.Connector)
                _gameFactory.CreateConstruction<ConnectDivideMonsters>(_spawnPlaces[_nowConstructions].transform.position);

            else if (_constructionType == Constructions.MilitaryOffice)
                _gameFactory.CreateConstruction<MilitaryOffice>(_spawnPlaces[_nowConstructions].transform.position);

            _nowConstructions++;

            UpdateButton();

        }

        private void UpdateButton()
        {
            int newCost = _nowConstructions * _costUpdate;
            if (newCost > _playerProgress.MoneyAmount)
            {
                _button.interactable = false;
            }
            else
            {
                _button.interactable = true;
                
            }
            if (_nowConstructions >= _spawnPlaces.Length)
            {
                _button.interactable = false;
            }
            else
            {
                _textCost.text = newCost.ToString();
            }
        }
    }

    public enum Constructions
    {
        Incubator,
        Connector,
        MilitaryOffice
    }
}