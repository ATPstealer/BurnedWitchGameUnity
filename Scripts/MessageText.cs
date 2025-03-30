using TMPro;
using UnityEngine;

public class MessageText : MonoBehaviour
{
    private TextMeshProUGUI _messageText;
    
    private void Awake()
    {
        // Get the Text component attached to this GameObject
        _messageText = GetComponent<TextMeshProUGUI>();
        _messageText.text = Store.MessageUI;
    }
    
    private void OnEnable()
    {
        // Subscribe to the OnScoreChanged event
        Store.OnMessageUIChanged += UpdateMessageUIText;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnScoreChanged event to prevent memory leaks
        Store.OnMessageUIChanged -= UpdateMessageUIText;
    }

    private void UpdateMessageUIText(string newMessage)
    {
        // Update the text display with the new score
        _messageText.text = newMessage;
    }
}
