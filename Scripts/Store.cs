using UnityEngine;

public class Store : MonoBehaviour
{
    public static int level = 0;
    public static bool passed = false;
    public static bool lockInputs = false;
    public static bool dead = false;
    public static bool win = false;
    public static bool lose = false;
    public static float manaMax = 100;
    public static float manaRegen = 10f;
    public static float maxSpeed = 20f;
    
    // Message in right upper corner
    public static string MessageUI
    {
        get => _messageUI;
        set
        {
            if (value != _messageUI) 
            {
                _messageUI = value;
                OnMessageUIChanged?.Invoke(_messageUI);
            }
        }
    }
    private static string _messageUI = ""; 
    public static event System.Action<string> OnMessageUIChanged;

    
    // Mana
    public static float Mana
    {
        get => _mana;
        set
        {
            if (Mathf.Abs(_mana - value) > Mathf.Epsilon) // Avoid event triggers on very tiny value changes
            {
                _mana = value;
                OnManaChanged?.Invoke(_mana);
            }
        }
    }

    private static float _mana = 100f; 
    public static event System.Action<float> OnManaChanged;
    // Lives 
    public static int Lives
    {
        get => _lives;
        set
        {
            if (_lives != value)
            {
                _lives = value;
                OnLivesChanged?.Invoke(_lives);
            }
        }
    }
    private static int _lives = 3;
    public static event System.Action<int> OnLivesChanged;
    // Score 
    public static int Score
    {
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = value;
                OnScoreChanged?.Invoke(_score); 
            }
        }
    }
    private static int _score = 0;
    public static event System.Action<int> OnScoreChanged;
}
