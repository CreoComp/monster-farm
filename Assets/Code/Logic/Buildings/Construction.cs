using System;
using System.Collections.Generic;
using Code.Infrastructure.GameFactory;
using Code.Infrastructure.Services.PlayerProgressService;
using Code.Logic.Monster;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Logic.Buildings
{
    public abstract class Construction : MonoBehaviour
    {
        [SerializeField] protected MonsterPlatform Platform;
        [SerializeField] protected Transform SpawnPoint;
        [SerializeField] protected TextMeshProUGUI TimerText;
        [FormerlySerializedAs("_canvas")] [SerializeField] protected Canvas Canvas;

        protected IGameFactory GameFactory;
        protected IPlayerProgressService PlayerProgressService;

        private bool _isActiveCanvas;
        protected float Time;

        protected SoundDataService SoundDataService;

        public virtual void Construct(IGameFactory gameFactory, IPlayerProgressService playerProgressService, SoundDataService soundDataService)
        {
            GameFactory = gameFactory;
            SoundDataService = soundDataService;
            PlayerProgressService = playerProgressService;
        }
        
        private void OnEnable()
        {
            Platform.PlatformIsFull += OnConstructionIsFull;
            Platform.MonstersCountUpdated += OnMonsterCountUpdate;
        }

        private void OnDisable()
        {
            Platform.PlatformIsFull -= OnConstructionIsFull;
            Platform.MonstersCountUpdated -= OnMonsterCountUpdate;
        }

        protected abstract void OnConstructionIsFull(List<MonsterStateMachine> monsterStateMachines);
        protected abstract void OnMonsterCountUpdate(List<MonsterStateMachine> monsterStateMachines);

        public abstract void InvokeDestinedAction();

        public void SetCanvasActive()
        {
            if (Canvas == null)
                return;

            Canvas.gameObject.SetActive(true);
            _isActiveCanvas = true;
        }



        protected virtual void Update()
        {
            UpdateCanvas();

            if (TimerText == null)
                return;

            if (Time <= 0)
            {
                TimerText.gameObject.SetActive(false);
                return;
            }
            else
                TimerText.gameObject.SetActive(true);
            
            TimerText.text = Math.Round(Time, 1) + "s";
        }

        private void UpdateCanvas()
        {
            if (Canvas == null)
                return;

            if (!_isActiveCanvas)
                Canvas.gameObject.SetActive(false);
            _isActiveCanvas = false;
        }
    }
}