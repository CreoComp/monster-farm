namespace Code.Logic.Monster
{
    public class MonstersMilitaryCampaign
    {
        public int RewardMoney { get; }
        public float RecommendedBattleStrengthValue { get; }
        public float RequiredTimeToCompleteCampaign { get; }

        public MonstersMilitaryCampaign(int rewardMoney, int recommendedBattleStrengthValue, float requiredTimeToCompleteCampaign)
        {
            RewardMoney = rewardMoney;
            RecommendedBattleStrengthValue = recommendedBattleStrengthValue;
            RequiredTimeToCompleteCampaign = requiredTimeToCompleteCampaign;
        }
    }
}