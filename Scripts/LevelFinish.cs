using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private int score = 5;
 
    private bool nextTrigger = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(Store.level);
        if (collision.gameObject.CompareTag("Cat") && !nextTrigger)
        {
            nextTrigger = true;
            Store.Score += score;
            Store.level++;  
            SceneManager.LoadScene(Store.level);
        }
    }
}
