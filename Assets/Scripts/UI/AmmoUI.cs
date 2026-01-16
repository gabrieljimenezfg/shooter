using System;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentAmmo, reserve;

    private void Start()
    {
        Weapon.OnAmmoChanged += UpdateBullets;
        UpdateBullets();
    }

    private void OnDestroy()
    {
        Weapon.OnAmmoChanged -= UpdateBullets;
    }

    private void UpdateBullets()
    {
        var magazineBullets = GameManager.Instance.GetEquippedWeapon().MagazineBullets;
        var totalBullets = GameManager.Instance.GetEquippedWeapon().TotalBullets;

        currentAmmo.text = magazineBullets;
        reserve.text = totalBullets;
    }
}