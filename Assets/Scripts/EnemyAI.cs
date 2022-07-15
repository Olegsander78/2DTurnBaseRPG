using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private AnimationCurve HealChanceCurve;
    [SerializeField] private Character character;

    private void OnEnable()
    {
        TurnManager.Instance.OnBeginTurn += OnBeginTurn;
    }

    private void OnDisable()
    {
        TurnManager.Instance.OnBeginTurn -= OnBeginTurn;
    }

    private void OnBeginTurn(Character c)
    {
        if (character == c)
        {
            DetermineCombatAction();
        }
    }

    private void DetermineCombatAction()
    {
        float healthPercentage = character.GetHealthPercentage();
        bool wantToHeal = Random.value < HealChanceCurve.Evaluate(healthPercentage);

        CombatAction ca = null;

        if (wantToHeal && HasCombatActionOfType(CombatAction.Type.Heal))
        {
            ca = GetCombatActionOfType(CombatAction.Type.Heal);
        }
        else if (HasCombatActionOfType(CombatAction.Type.Attack))
        {
            ca = GetCombatActionOfType(CombatAction.Type.Attack);
        }

        if (ca != null)
            character.CastCombatAction(ca);
        else
            TurnManager.Instance.EndTurn();
    }

    private bool HasCombatActionOfType(CombatAction.Type type)
    {
        return character.CombatActions.Exists(x => x.ActionType == type);
    }

    private CombatAction GetCombatActionOfType(CombatAction.Type type)
    {
        List<CombatAction> availableActions = character.CombatActions.FindAll(x => x.ActionType == type);
        return availableActions[Random.Range(0, availableActions.Count)];
    }
}
