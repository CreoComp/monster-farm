using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Logic.Monster.MonsterData
{
    [CreateAssetMenu(menuName = "MonsterData/NewMonster", fileName = "NewMonster")]
    public class MonsterData : ScriptableObject
    {
        [FormerlySerializedAs("_rareLevel")] [SerializeField] private MonsterRarityLevel _rarityLevel;
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _monsterPrefab;

        public MonsterRarityLevel RarityLevel => _rarityLevel;
        public Sprite Icon => _icon;
        public GameObject Prefab => _monsterPrefab;

    }
}