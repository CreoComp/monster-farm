using Code.Logic.Buildings;
using Code.Logic.Monster;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IdentifierBattleValue : Construction
{
    [SerializeField] private TextMeshProUGUI textValueBattle;

    private List<MonsterStateMachine> _monsterStateMachines;

    public override void InvokeDestinedAction()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnConstructionIsFull(List<MonsterStateMachine> monsterStateMachines)
    {
        _monsterStateMachines = monsterStateMachines;
        SetActiveTextValue();
    }

    protected override void OnMonsterCountUpdate(List<MonsterStateMachine> monsterStateMachines)
    {
        _monsterStateMachines = monsterStateMachines;
        if (_monsterStateMachines.Count <= 0)
        {
            textValueBattle.gameObject.SetActive(false);

            return;
        }
        SetActiveTextValue();

    }

    public void SetActiveTextValue()
    {
        textValueBattle.gameObject.SetActive(true);

        textValueBattle.text = _monsterStateMachines[0].GetComponent<MonsterBattleStrength>().BattleStrengthValue + "";
    }
}
