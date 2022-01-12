using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChipBuff
{
    [SerializeField] public BuffType buffType;
    [SerializeField] public float addiditiveValue = 0;
}

public enum BuffType
{
    Health,
    DodgeRecharge,
    AttackStrength,
    MovementSpeed
}