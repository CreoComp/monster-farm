using UnityEngine;

namespace Code.Logic.Monster.MonsterData
{
    [CreateAssetMenu(menuName = "MonsterData/RareLevel", fileName = "RareLevel")]
    public class MonsterRarityLevel : ScriptableObject
    {
        [SerializeField] private int _minBattleStrengthValue;
        [SerializeField] private int _maxBattleStrengthValue;
        [SerializeField] private string _levelName;
        [SerializeField] private Color _outlineColor;

        public string LevelName => _levelName;
        public int MinBattleStrengthValue => _minBattleStrengthValue;
        public int MaxBattleStrengthValue => _maxBattleStrengthValue;

        public Color OutlineColor => _outlineColor;
    }
}