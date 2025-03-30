using UnityEngine;

public class Store : MonoBehaviour
{
    public static int level = 0;
    public static bool passed = false;
    public static bool lockInputs = false;
    public static bool dead = false;
    public static bool win = false;
    public static bool lose = false;
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
