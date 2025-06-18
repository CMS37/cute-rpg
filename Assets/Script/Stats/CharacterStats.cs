using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    [Header("스탯")]
    public StatAttribute hp;
    public StatAttribute attack;
    public StatAttribute defense;

    private int currentHP;

    public event Action      OnDeath;
    public event Action<int> OnDamaged;

    private void Awake()
    {
        currentHP = hp.Current;
    }

    public void TakeDamage(int rawDamage)
    {
        int dmg = Mathf.Max(0, rawDamage - defense.Current);

        OnDamaged?.Invoke(dmg);
        
        if (dmg <= 0) return;
        currentHP -= dmg;

        Debug.Log($"{name} take {dmg}dmg, left {currentHP}hp");
        if (currentHP <= 0)
        {
            currentHP = 0;
            OnDeath?.Invoke();
        }
    }

    public int MaxHP          => hp.Current;
    public int CurrentHP      => currentHP;
    public int CurrentAttack  => attack.Current;
    public int CurrentDefense => defense.Current;

    public void SetCurrentHP(int value)
    {
        currentHP = Mathf.Clamp(value, 0, MaxHP);
    }

    public void SetMaxHP(int max)
    {
        hp.baseValue = max - hp.bonusValue;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
    }

    public void SetAttack(int atk)
    {
        attack.baseValue = atk - attack.bonusValue;
    }

    public void SetDefense(int def)
    {
        defense.baseValue = def - defense.bonusValue;
    }
}

[System.Serializable]
public class StatAttribute
{
    public int baseValue;
    public int bonusValue;

    public int Current
    {
        get => baseValue + bonusValue;
        set => bonusValue = value - baseValue;
    }
}
