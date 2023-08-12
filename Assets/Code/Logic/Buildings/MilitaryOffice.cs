using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Code.Logic.Monster;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Logic.Buildings
{
    public class MilitaryOffice : Construction
    {
        [SerializeField] private TMP_Text _campaignResultSuccessText;
        [SerializeField] private TMP_Text _campaignFailedResultText;
        [Space]
        
        [SerializeField] private TMP_Text _recommendedBattleStrengthValueText;
        [SerializeField] private TMP_Text _requiredTimeText;
        [SerializeField] private TMP_Text _rewardText;
        [Space]
        
        [SerializeField] private int _minCampaignReward = 10;
        [SerializeField] private int _maxCampaignReward = 100;
        [SerializeField] private int _rewardMultiplier = 10;
        
        [Space]
        [SerializeField] private int _minRecommendBattleStrengthValue = 50;
        [SerializeField] private int _maxRecommendBattleStrengthValue = 300;
        [SerializeField] private int _recommendedBattleStrengthValueMultiplier = 10;
        
        [Space] 
        [SerializeField] private float _minTimeToPass = 30f;
        [SerializeField] private float _maxTimeToPass = 60f;
        [SerializeField] private float _requiredTimeMultiplier = 10f;
        
        [Space] 
        [SerializeField, Range(0, 1f)] private float _maxWinChance = 0.8f;
        
        private MonstersMilitaryCampaign _currentCampaign;
        private int _passedCampaignsAmount;
        
        private Coroutine _coroutine;
        private bool _isCampaign;

        private List<MonsterStateMachine> _monsterStateMachines;

        private float _battleStrengthValueOfParty;
        private int _originPlatformCapacity;

        private void Start()
        {
            CreateNewCampaign();
        }

        protected override void OnConstructionIsFull(List<MonsterStateMachine> monsterStateMachines)
        {
            
        }

        protected override void OnMonsterCountUpdate(List<MonsterStateMachine> monsterStateMachines)
        {
            _monsterStateMachines = monsterStateMachines;
        }

        public override void InvokeDestinedAction()
        {
            if (_monsterStateMachines.Count <= 0)
                return;

            if (GameFactory.ActiveMonsters.Count == _monsterStateMachines.Count && GameFactory.ActiveEggs.Count < 1)
            {
                Debug.Log("!");
                return;
            }
            
            var sumOfBattleStrengthValue = CalculateBattleStrengthValueSumAndRemoveMonsters(_monsterStateMachines);
            StartNewCampaign(sumOfBattleStrengthValue);
        }

        private int CalculateBattleStrengthValueSumAndRemoveMonsters(List<MonsterStateMachine> monsterStateMachines)
        {
            int sumOfBattleStrengthValue = 0;
            foreach (var monsterStateMachine in monsterStateMachines)
            {
                var monsterBattleStrength = monsterStateMachine.GetComponent<MonsterBattleStrength>();
                sumOfBattleStrengthValue += monsterBattleStrength.BattleStrengthValue;
                GameFactory.RemoveMonster(monsterBattleStrength);
            }

            Platform.ClearCurrentMonstersList();
            
            return sumOfBattleStrengthValue;
        }

        private void StartNewCampaign(float battleStrengthValueOfParty)
        {
            _isCampaign = true;
            
            _battleStrengthValueOfParty = battleStrengthValueOfParty;
            Time = _currentCampaign.RequiredTimeToCompleteCampaign;
            
            _originPlatformCapacity = Platform.MaxCapacity;
            Platform.SetMaxCapacity(0);
        }



        private void CreateNewCampaign()
        {
            int reward = Mathf
                .Clamp(_minCampaignReward + _passedCampaignsAmount * _rewardMultiplier,_minCampaignReward, _maxCampaignReward);
            int recommendedBattleStrength = Mathf
                .Clamp(_minRecommendBattleStrengthValue + _passedCampaignsAmount * _recommendedBattleStrengthValueMultiplier, min: _minRecommendBattleStrengthValue , max: _maxRecommendBattleStrengthValue);;
            float time = Mathf
                .Clamp(_minTimeToPass + _passedCampaignsAmount * _requiredTimeMultiplier, min: _minTimeToPass, max: _maxTimeToPass);
            
            _currentCampaign = new MonstersMilitaryCampaign(reward, recommendedBattleStrength, time);

            UpdateUI();
        }

        private void UpdateUI()
        {
            _rewardText.text = _currentCampaign.RewardMoney.ToString();
            _requiredTimeText.text = _currentCampaign.RequiredTimeToCompleteCampaign.ToString(CultureInfo.InvariantCulture);
            _recommendedBattleStrengthValueText.text = _currentCampaign.RecommendedBattleStrengthValue.ToString(CultureInfo.InvariantCulture);
        }

        protected override void Update()
        {
            base.Update();

            if (_isCampaign)
                UpdateCampaignTimer();
        }

        private void UpdateCampaignTimer()
        {
            Time -= UnityEngine.Time.deltaTime;
            if (Time <= 0)
            {
                EndCampaign();
            }
        }

        private void EndCampaign()
        {
            Time = 0;
            _isCampaign = false;

            float chanceToSuccess =
                Mathf.Clamp(_battleStrengthValueOfParty / _currentCampaign.RecommendedBattleStrengthValue, 0f,
                    _maxWinChance) * 100;
            Debug.Log($"<color=red> {chanceToSuccess} </color>");
            if (Random.Range(0, 101) <= chanceToSuccess)
            {
                FinishCampaignAsSuccessful();
            }
            else
            {
                FinishCampaignAsFailed();
            }
            Platform.SetMaxCapacity(_originPlatformCapacity);
            _passedCampaignsAmount++;
            CreateNewCampaign();
        }

        private void FinishCampaignAsSuccessful()
        {
            PlayerProgressService.AddMoney(_currentCampaign.RewardMoney);
            Debug.Log("<color=green> Successful </color>");
            StartCoroutine(ShowTextForTime(_campaignResultSuccessText));
        }

        private void FinishCampaignAsFailed()
        {
            Debug.Log("<color=red> Failed </color>");
            StartCoroutine(ShowTextForTime(_campaignFailedResultText));
        }

        private IEnumerator ShowTextForTime(TMP_Text text, float time = 2f)
        {
            text.gameObject.SetActive(true);
            yield return new WaitForSeconds(time);
            text.gameObject.SetActive(false);
        }
    }
}