using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField] private float currentLife;
    [SerializeField] private float maxLife;
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private int equippedWeaponIndex;

    public float CurrentLife
    {
        get => currentLife;
        set => currentLife = value;
    }
    
    public float MaxLife 
    {
        get => maxLife;
        set => maxLife = value;
    }

    public List<Weapon> Weapons
    {
        get => weapons;
        set => weapons = value;
    }

    public int EquippedWeaponIndex
    {
        get => equippedWeaponIndex;
        set => equippedWeaponIndex = value;
    }
}