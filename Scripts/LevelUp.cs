using System.Collections;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private void Awake()
    {
    }
    
    private void OnEnable()
    {
        // Subscribe to the OnScoreChanged event
        Store.OnScoreChanged += UpdateLevelUp;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnScoreChanged event to prevent memory leaks
        Store.OnScoreChanged -= UpdateLevelUp;
    }

    private void UpdateLevelUp(int newScore)
    {
        if (Store.Score >= 5*Store.heroLevel && newScore >= 5*Store.heroLevel)
        {
            StartCoroutine(LevelUpHandle());
        }
    }
    
    IEnumerator LevelUpHandle()
    {
        yield return new WaitForSeconds(Time.deltaTime * 2);
        Store.Score -= 5 * Store.heroLevel;
        Store.heroLevel++;
        Store.Lives++;
        Store.manaMax += 10;

        Store.MessageUI = "Level up!!!";
        Score.UpdateScoreText(Store.Score);
        StartCoroutine(CleanMessage());
    }
    
    IEnumerator CleanMessage()
    {
        yield return new WaitForSeconds(5);
        Store.MessageUI = "";
    }
}