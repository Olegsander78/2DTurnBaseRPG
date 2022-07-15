using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public int CurHP;
    public int MaxHP;

    public bool IsPlayer;

    public List<CombatAction> CombatActions = new List<CombatAction>();

    [SerializeField] private Character _opponent;

    private Vector3 _startPos;

    public event UnityAction OnHealthChange;
    public static event UnityAction<Character> OnDie; 

    private void Start()
    {
        _startPos = transform.position;
    }

    public void TakeDamage(int damageToTake)
    {
        CurHP -= damageToTake;

        OnHealthChange?.Invoke();

        if (CurHP <= 0)
            Die();
    }

    private void Die()
    {
        OnDie?.Invoke(this);
        Destroy(gameObject);
    }

    public void Heal(int healAmount)
    {
        CurHP += healAmount;

        OnHealthChange?.Invoke();

        if (CurHP > MaxHP)
            CurHP = MaxHP;
    }

    public void CastCombatAction(CombatAction combatAction)
    {
        if (combatAction.Damage > 0)
        {
            StartCoroutine(AttackOpponent(combatAction));
        }
        else if (combatAction.ProjectitlePrefab != null)
        {

        }
        else if (combatAction.HealAmount > 0)
        {
            Heal(combatAction.HealAmount);
            TurnManager.Instance.EndTurn();
        }
    }

    IEnumerator AttackOpponent(CombatAction combatAction)
    {
        while (transform.position != _opponent.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _opponent.transform.position, 50f * Time.deltaTime);
            yield return null;
        }

        _opponent.TakeDamage(combatAction.Damage);

        while (transform.position != _startPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, 20f * Time.deltaTime);
            yield return null;
        }

        TurnManager.Instance.EndTurn();
    }

    public float GetHealthPercentage()
    {
        return (float)CurHP / (float)MaxHP;
    }
}
