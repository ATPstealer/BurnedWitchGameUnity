using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private int score = 5;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cat"))
        {
            Store.Score += score;
            Store.level++;
            SceneManager.LoadScene(Store.level);
        }
    }
}
