using UnityEngine;

namespace Code.Logic.Monster.MonsterStates
{
    public interface ICollisionState
    {
        public void OnCollisionEnter(Collision collision);
    }
}