using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        // Get the Text component attached to this GameObject
        _scoreText = GetComponent<TextMeshProUGUI>();
    }
    
    private void OnEnable()
    {
        // Subscribe to the OnScoreChanged event
        Store.OnScoreChanged += UpdateScoreText;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnScoreChanged event to prevent memory leaks
        Store.OnScoreChanged -= UpdateScoreText;
    }

    private void UpdateScoreText(int newScore)
    {
        // Update the text display with the new score
        _scoreText.text = "Score: " + newScore;
    }

}
