using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public enum PlayerTurn { White, Black }
    
    [Header("Game Settings")]
    [SerializeField] private float _stoppedThreshold = 0.05f;
    [SerializeField] private float _checkInterval = 0.2f;
    
    private PlayerTurn _currentTurn = PlayerTurn.White;
    private List<Rigidbody> _allCheckers = new List<Rigidbody>();
    private bool _isMovementActive = false;
    private float _lastCheckTime;

    private TextMeshProUGUI turnTmp;

    public TextMeshProUGUI timerB;
    public TextMeshProUGUI timerW;

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

    private void Start()
    {
        // Получаем все шашки на сцене
        DragShoot[] checkers = FindObjectsOfType<DragShoot>();
        foreach (DragShoot checker in checkers)
        {
            Rigidbody rb = checker.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _allCheckers.Add(rb);
            }
        }
        TurnTimer.Instance.StartTimer(timerW);
        turnTmp = GameObject.FindGameObjectWithTag("TurnTxt").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(TurnTimer.Instance.GetCurrentTime() >= 6f)
            SwitchTurn();


        // Проверяем состояние движения только если есть активное движение
            if (_isMovementActive && Time.time > _lastCheckTime + _checkInterval)
            {
                _lastCheckTime = Time.time;

                TurnTimer.Instance.StopTimer();

                if (AllCheckersStopped())
                {
                    _isMovementActive = false;
                    SwitchTurn();
                }
            }
    }

    // Вызывается при ударе по шашке
    public void OnCheckerStruck()
    {
        _isMovementActive = true;
        _lastCheckTime = Time.time;
    }

    private bool AllCheckersStopped()
    {
        foreach (Rigidbody rb in _allCheckers)
        {
            if (rb == null) continue;
            
            if (rb.linearVelocity.magnitude > _stoppedThreshold || 
                rb.angularVelocity.magnitude > _stoppedThreshold)
            {
                return false;
            }
        }
        return true;
    }

    private void SwitchTurn()
    {
        if (_currentTurn == PlayerTurn.White)
        {
            _currentTurn = PlayerTurn.Black;
            turnTmp.text = "Black";
        }
        else
        {
            _currentTurn = PlayerTurn.White;
            turnTmp.text = "White";
        }
        Debug.Log($"Ход передан: {_currentTurn}");
        if (_currentTurn == PlayerTurn.White)
        {
            TurnTimer.Instance.ResetTimer();
            TurnTimer.Instance.StartTimer(timerW);
        }
        else
        {
            TurnTimer.Instance.ResetTimer();
            TurnTimer.Instance.StartTimer(timerB);
        }
    }

    public bool CanStrike()
    {
        return !_isMovementActive;
    }

    public PlayerTurn CurrentTurn => _currentTurn;
}