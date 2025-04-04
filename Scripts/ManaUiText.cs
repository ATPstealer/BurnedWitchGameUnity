using TMPro;
using UnityEngine;

public class ManaUiText : MonoBehaviour
{
    private TextMeshProUGUI _manaText;

    private void Awake()
    {
        _manaText = GetComponent<TextMeshProUGUI>();
        _manaText.text = "100/100";
    }
    
    private void OnEnable()
    {
        Store.OnManaChanged += UpdateManaText;
    }

    private void OnDisable()
    {
        Store.OnManaChanged -= UpdateManaText;
    }

    private void UpdateManaText(float newMana)
    {
        _manaText.text = (int)newMana + "/" + (int)Store.manaMax;
    }

}