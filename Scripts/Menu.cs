using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void NewGameButton()
    {
        Store.level = 1;
        Store.Score = 0;
        Store.Lives = 3;
        SceneManager.LoadScene(Store.level);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(Store.level);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
