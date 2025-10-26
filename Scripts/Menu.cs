using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if (continueButton != null)
        {
            continueButton.SetActive(Store.level != 0);
        }
    }

    public void NewGameButton()
    {
        Store.level = 1;
        Store.Score = 0;
        Store.Lives = 3;
        Store.MessageUI = "";
        SceneManager.LoadScene(Store.level);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(Store.level);
    }

    public void QuitButton()
    {
        // Close the Unity Activity
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("finish");

        // Kill the app process
        AndroidJavaClass process = new AndroidJavaClass("android.os.Process");
        int pid = process.CallStatic<int>("myPid");
        process.CallStatic("killProcess", pid);

        // Optionally quit the Unity application
        System.Environment.Exit(0);

    }
}
