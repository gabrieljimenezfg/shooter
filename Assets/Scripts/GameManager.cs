using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameData gameData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameData GetGameData
    {
        get { return gameData; }
        set { gameData = value; }
    }

    public Weapon GetEquippedWeapon()
    {
        var carriedWeapons = GetGameData.Weapons;
        var equippedWeaponIndex = GetGameData.EquippedWeaponIndex;
        return carriedWeapons[equippedWeaponIndex];
    }
}