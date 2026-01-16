using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameData gameData;

    public event Action<float, float> HealthUpdated;

    [SerializeField] private float healingSpeed;
    [SerializeField] private float timeToStartHealing;
    private Coroutine healingCoroutine;

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

    private void Start()
    {
        PlayerController.TookDamage += PlayerTakeDamage;
    }

    private void OnDestroy()
    {
        PlayerController.TookDamage -= PlayerTakeDamage;
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

    private void PlayerTakeDamage(float damage)
    {
        if (healingCoroutine != null)
        {
            StopCoroutine(healingCoroutine);
        }

        gameData.CurrentLife -= damage;
        if (gameData.CurrentLife >= 0)
        {
            healingCoroutine = StartCoroutine(nameof(Heal));
        }

        HealthUpdated?.Invoke(gameData.CurrentLife, gameData.MaxLife);
    }

    private IEnumerator Heal()
    {
        yield return new WaitForSeconds(timeToStartHealing);
        while (gameData.CurrentLife < gameData.MaxLife)
        {
            var newLife = gameData.CurrentLife + (healingSpeed * Time.deltaTime);
            gameData.CurrentLife = Mathf.Clamp(newLife, 0, gameData.MaxLife);
            HealthUpdated?.Invoke(gameData.CurrentLife, gameData.MaxLife);
            yield return null;
        }
    }
}