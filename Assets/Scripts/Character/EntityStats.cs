using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField] float baseMaxHitpoints = 100;
    [SerializeField] float baseMovementSpeed = 5;

    List<ChipBuff> buffs = new List<ChipBuff>();

    public float MaxHitpoints
    {
        get
        {
            return baseMaxHitpoints + GetBuffAdditive(BuffType.Health);
        }
    }

    public float MovementSpeed
    {
        get
        {
            return baseMovementSpeed + GetBuffAdditive(BuffType.MovementSpeed);
        }
    }

    private void Awake()
    {
        buffs = new List<ChipBuff>();
    }

    public void ResetBuffs()
    {
        buffs.Clear();
    }

    public void AddBuff(ChipBuff[] toAdd)
    {
        buffs.AddRange(toAdd);
    }

    public float GetBuffAdditive(BuffType buffType)
    {
        float total = 0;
        foreach (ChipBuff buff in buffs)
        {
            if (buff.buffType == buffType)
            {
                total += buff.addiditiveValue;
            }
        }

        return total;
    }
}