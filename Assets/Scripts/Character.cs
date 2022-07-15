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
    }
}
