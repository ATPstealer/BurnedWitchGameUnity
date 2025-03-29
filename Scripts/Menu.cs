using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void NewGameButton()
    {
        Store.level = 1;
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
