using System;
using System.Collections.Generic;
using Code.Logic.Monster;
using Code.Logic.Monster.MonsterStates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Logic.Buildings
{
    public class MonsterPlatform : MonoBehaviour
    {
        [FormerlySerializedAs("maxCapacity")] [SerializeField]
        private int _maxCapacity;

        public int MaxCapacity => _maxCapacity;
        
        private const float TimeToEggRelease = 3f;
        private float _time;

        public List<MonsterStateMachine> CurrentMonsters { get; private set; } = new();

        public Action<List<MonsterStateMachine>> PlatformIsFull;
        public Action<List<MonsterStateMachine>> MonstersCountUpdated;

        private bool _platformHasEgg;
        private bool _platformHasMaximumMonsters;

        public void AddToCurrent(MonsterStateMachine monster)
        {
            if (CurrentMonsters.Count < _maxCapacity && !IsPlatformFull())
            {
                monster.CurrentPlatform = this;
                CurrentMonsters.Add(monster);
                MonstersCountUpdated?.Invoke(CurrentMonsters);
                monster.EnterIn<IdleState>();
                Debug.Log($"{monster.name} added to list of current monsters on {name} platform");
            }
            else
            {
                monster.AddForceOutOfConstruction(transform.position);
                Debug.Log($"{monster.name} forced from {name} platform because of platform monsters limit");
            }

            if (CurrentMonsters.Count >= _maxCapacity)
            {
                _platformHasMaximumMonsters = true;
                PlatformIsFull?.Invoke(CurrentMonsters);
                Debug.Log($"{name} platform full and its event invoked");
            }
        }

        public void RemoveFromCurrent(MonsterStateMachine monster)
        {
            _platformHasMaximumMonsters = false;
            CurrentMonsters.Remove(monster);
            Debug.Log($"{monster.name} removed from {name} active monsters list");
        }

        public void ClearCurrentMonstersList()
        {
            _platformHasMaximumMonsters = false;
            CurrentMonsters.Clear();
            MonstersCountUpdated?.Invoke(CurrentMonsters);

            Debug.Log($"{name} cleared its current monsters list");
        }

        public void SetEggIntoPlatform(bool value)
        {
            _platformHasEgg = value;
        }

        public void SetMaxCapacity(int amount)
        {
            if (amount < 0)
            {
                Debug.Log("Invalid capacity size");
                return;
            }

            _maxCapacity = amount;
        }

        public bool IsPlatformFull()
        {
            if (_platformHasEgg || _platformHasMaximumMonsters)
                return true;
            else
                return false;
        }

        private void Update()
        {
            TimerEggRelease();
        }

        private void TimerEggRelease()
        {
            if (_platformHasEgg)
            {
                _time += Time.deltaTime;

                if (_time >= TimeToEggRelease)
                {
                    _time = 0;
                    _platformHasEgg = false;
                }
            }
        }
    }
}