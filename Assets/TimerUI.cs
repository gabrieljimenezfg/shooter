using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private const float TenMinutes = 600f;
    [SerializeField] private TextMeshProUGUI countdownText;

    private float timeRemaining = TenMinutes;
    private bool isRunning = true;

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var minutes = Mathf.FloorToInt(timeRemaining / 60f);
        var seconds = Mathf.FloorToInt(timeRemaining % 60f);
        countdownText.text = $"{minutes:00}:{seconds:00}";
    }
}