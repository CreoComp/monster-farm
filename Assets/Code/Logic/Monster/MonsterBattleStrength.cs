using UnityEngine;

namespace Code.Logic.Monster
{
    public class MonsterBattleStrength : MonoBehaviour
    {
        public int BattleStrengthValue { get; private set; }

        public void Construct(int minStrengthValue, int maxStrengthValue)
        {
            BattleStrengthValue = Random.Range(minStrengthValue, maxStrengthValue);
        }
    }
}