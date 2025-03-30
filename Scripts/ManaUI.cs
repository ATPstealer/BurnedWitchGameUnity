using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    public Image manaBar;

    private void Start()
    {
        manaBar.fillAmount = Store.Mana / 100f;
        Store.OnManaChanged += UpdateManaBar;
    }

    private void OnDestroy()
    {
        Store.OnManaChanged -= UpdateManaBar;
    }
    
    private void UpdateManaBar(float newMana)
    {
        manaBar.fillAmount = newMana / Store.manaMax;
    }
}