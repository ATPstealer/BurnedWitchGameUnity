using TMPro;
using UnityEngine;

public class Lives : MonoBehaviour
{
    private TextMeshProUGUI _livesText;
    
    private void Awake()
    {
        // Get the Text component attached to this GameObject
        _livesText = GetComponent<TextMeshProUGUI>();
        _livesText.text = "Lives: " + Store.Lives;
    }
    
    private void OnEnable()
    {
        // Subscribe to the OnScoreChanged event
        Store.OnLivesChanged += UpdateLivesText;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnScoreChanged event to prevent memory leaks
        Store.OnLivesChanged -= UpdateLivesText;
    }

    private void UpdateLivesText(int newLives)
    {
        // Update the text display with the new score
        _livesText.text = "Lives: " + newLives;
    }

}