using UnityEngine;

namespace Code.Logic.Monster.MonsterStates
{
    public interface ITriggerState
    {
        void OnTriggerEnter(Collider other);
        void OnTriggerExit(Collider other);
    }
}