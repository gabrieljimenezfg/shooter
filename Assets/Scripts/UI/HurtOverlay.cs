using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HurtOverlay : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;

    private void Start()
    {
        volume.profile.TryGet(out vignette);
        GameManager.Instance.HealthUpdated += GameManagerHealthUpdated;
    }

    private void GameManagerHealthUpdated(float currentLife, float maxLife)
    {
        var percentage = 1 - (currentLife / maxLife);
        vignette.intensity.value = percentage;
    }
}