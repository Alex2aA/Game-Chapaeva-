using TMPro;
using UnityEngine;

public class TurnTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    public static TurnTimer Instance { get; private set; }

    private float _elapsedTime;
    private bool _isRunning;

    private void Awake()
    {
        // Singleton реализация
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        int seconds = (int)(_elapsedTime % 60);
        int milliseconds = (int)((_elapsedTime * 1000) % 1000);
        
        timerText.text = $"{seconds:00}.{milliseconds:000}";
    }

    public void StartTimer(TextMeshProUGUI _timerText)
    {
        timerText = _timerText;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    public float GetCurrentTime()
    {
        return _elapsedTime;
    }
}
