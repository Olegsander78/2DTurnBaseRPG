using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatActionUI : MonoBehaviour
{
    [SerializeField] private GameObject VisualContainer;
    [SerializeField] private Button[] CombatActionButtons;

    private void OnEnable()
    {
        TurnManager.Instance.OnBeginTurn += OnBeginTurn;
        TurnManager.Instance.OnEndTurn += OnEndTurn;
    }

    private void OnDisable()
    {
        TurnManager.Instance.OnBeginTurn -= OnBeginTurn;
        TurnManager.Instance.OnEndTurn -= OnEndTurn;
    }

    private void OnBeginTurn(Character character)
    {
        if (!character.IsPlayer)
            return;

        VisualContainer.SetActive(true);

        for (int i = 0; i < CombatActionButtons.Length; i++)
        {
            if (i < character.CombatActions.Count)
            {
                CombatActionButtons[i].gameObject.SetActive(true);
                CombatAction ca = character.CombatActions[i];

                CombatActionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = ca.DisplayName;
                CombatActionButtons[i].onClick.RemoveAllListeners();
                CombatActionButtons[i].onClick.AddListener(()=> OnClickCombatAction(ca));
            }
            else
            {
                CombatActionButtons[i].gameObject.SetActive(false);
            }

        }
    }

    private void OnEndTurn(Character character)
    {
        VisualContainer.SetActive(false);
    }

    public void OnClickCombatAction(CombatAction combatAction)
    {
        TurnManager.Instance.CurrentCharacter.CastCombatAction(combatAction);
    }
}
